/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AutoGuida : NetworkBehaviour
{

    private bool RibaltaDisabilitato = false;

    private Quaternion OriginalRotation;
    private Quaternion[] WheelErrorCorrectionR = new Quaternion[4];
    private WheelCollider[] Colliders = new WheelCollider[4];
    private GameObject[] Wheels = new GameObject[4];

    private CameraManager MyCamera;
    private Transform LookHere, Position, AimPosition, CentroDiMassa;
    private Rigidbody TheCarRigidBody;

    //The class that owns the stats of the faction    
    private GeneralCar generalCar;

    private HudScriptManager HUD;
    private int Decellerazione = 0;
    private const int Moltiplicatore = 10;


    public override void OnStartLocalPlayer()
    {
        var i = 0;
        var wc = GetComponentsInChildren<WheelCollider>();
        var obj_figli = GetComponentsInChildren<MeshRenderer>();

        foreach (var w in wc)
        {
            foreach (var o in obj_figli)
                if (o.gameObject.name.Equals($"Wheel_{w.name}"))
                {
                    WheelErrorCorrectionR[i] = o.gameObject.transform.rotation;
                    Colliders[i] = w;
                    Wheels[i] = o.gameObject;
                    break;
                }

            i++;
        }

        generalCar = GetComponent<GeneralCar>();
        generalCar.Health = generalCar.Max_Health;

        TheCarRigidBody = GetComponent<Rigidbody>();
        MyCamera = Camera.main.GetComponent<CameraManager>();
        CentroDiMassa = transform.Find("CentroDiMassa");
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        AimPosition = transform.Find("CameraAnchor/AimPosition");
        OriginalRotation = TheCarRigidBody.transform.rotation;

        var HUDo = GameObject.FindGameObjectWithTag("HUD");
        HUD = HUDo.GetComponent<HudScriptManager>();

        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
        MyCamera.AimPosition = AimPosition;


        var difCentro = CentroDiMassa.position - transform.position;
        TheCarRigidBody.centerOfMass = difCentro;
    }

    private IEnumerator AbilitaRibalta()
    {
        yield return new WaitForSeconds(4);
        RibaltaDisabilitato = false;
    }

    private float fullBrake, handBrake, instantSteeringAngle, instantTorque;

    void Update()
    {
        if (isLocalPlayer)
        {
            if (!RibaltaDisabilitato)
            {
                var Ribalta = Input.GetKey(KeyCode.T);

                if (Ribalta)
                {
                    RibaltaDisabilitato = true;

                    var ppp = TheCarRigidBody.gameObject.transform.position;
                    TheCarRigidBody.gameObject.transform.SetPositionAndRotation(new Vector3(ppp.x, 0, ppp.z), OriginalRotation);

                    StartCoroutine(AbilitaRibalta());
                }
            }

            //freni
            fullBrake = (Input.GetKey(KeyCode.M) ? generalCar.brakingTorque : 0);
            handBrake = (Input.GetKey(KeyCode.K) ? generalCar.brakingTorque * 2 : 0);

            //DX-SX
            instantSteeringAngle = generalCar.maxSteeringAngle * Input.GetAxis("Horizontal");

            //Avanti-dietro
            instantTorque = generalCar.maxTorque * Input.GetAxis("Vertical");

            Decellerazione = (instantTorque == 0 ? 1 : 0);

            if (TheCarRigidBody.velocity.magnitude >= generalCar.Speed)
                instantTorque = 0;
        }
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Quaternion worldPose_rotation;
            Vector3 worldPose_position;

            for (var i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].tag.Equals("FrontWheel"))
                {
                    Colliders[i].steerAngle = instantSteeringAngle;
                    Colliders[i].motorTorque = instantTorque * Moltiplicatore;
                }

                if (fullBrake > 0)
                {
                    Colliders[i].brakeTorque = fullBrake * Moltiplicatore;
                }
                else if (handBrake > 0)
                {
                    if (Colliders[i].tag.Equals("BackWheel"))
                        Colliders[i].brakeTorque = handBrake * Moltiplicatore;
                }
                else
                {
                    Colliders[i].brakeTorque = 0 + Decellerazione;
                }

                //rotate the 3d object
                Colliders[i].GetWorldPose(out worldPose_position, out worldPose_rotation);

                Wheels[i].transform.position = worldPose_position;
                Wheels[i].transform.rotation = worldPose_rotation * WheelErrorCorrectionR[i];
            }

            generalCar.actualSpeed = TheCarRigidBody.velocity.magnitude;

            SetCannonsPositions();

            HUD.setValues(generalCar);
        }
    }

    private void SetCannonsPositions()
    {
        var weapons = GameObject.FindGameObjectsWithTag("Cannon");

        if (weapons != null)
        {
            var cars = GameObject.FindGameObjectsWithTag("car");

            if (cars != null)
                foreach (var w in weapons)
                    foreach (var c in cars)
                    {
                        var i = 0;

                        while (w.name[i] == c.name[i])
                            i++;

                        if (i > 3)
                        {
                            //è il mio cannone
                            var cannonPositionMarker = c.transform.Find("CannonPosition");
                            w.transform.position = cannonPositionMarker.position;
                            break;
                        }
                    }
        }
    }


}