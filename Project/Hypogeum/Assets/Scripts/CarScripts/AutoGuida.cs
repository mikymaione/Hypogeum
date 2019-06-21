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
    public enum eTrazione
    {
        anteriore, posteriore
    }

    public eTrazione Trazione = eTrazione.anteriore;

    private const short PosizionePavimento = -5;
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

    private Vector3 CentroDiMassaAssettoCorsa, CentroDiMassa3D;

    private HudScriptManager HUD;
    private int Decellerazione = 0;

    private float fullBrake, handBrake, instantSteeringAngle, instantTorque;

    //To manage the sand particle effect
    private ParticleSystem sandParticle;


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
        CmdSetMyHealt(gameObject, generalCar.Max_Health);

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

        CentroDiMassa3D = TheCarRigidBody.centerOfMass;
        CentroDiMassaAssettoCorsa = CentroDiMassa.position - transform.position;
        TheCarRigidBody.centerOfMass = CentroDiMassaAssettoCorsa;

        sandParticle = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    [Command] //only host
    private void CmdSetMyHealt(GameObject player, int v)
    {
        var p = player.GetComponent<GeneralCar>();
        p.Health = v;
    }

    private IEnumerator AbilitaRibalta()
    {
        yield return new WaitForSeconds(4);
        StopCoroutine(AbilitaRibalta());
        RibaltaDisabilitato = false;
    }

    private void EffettoVelocitaCamera()
    {
        Camera.main.fieldOfView = 60 + (TheCarRigidBody.velocity.magnitude / 3);
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            //freni
            fullBrake = (Input.GetKey(KeyCode.K) ? generalCar.brakingTorque : 0);
            handBrake = (Input.GetKey(KeyCode.M) ? generalCar.brakingTorque * 2 : 0);

            //DX-SX
            instantSteeringAngle = generalCar.maxSteeringAngle * Input.GetAxis("Horizontal");

            //Avanti-dietro
            instantTorque = generalCar.maxTorque * Input.GetAxis("Vertical");

            Decellerazione = (instantTorque == 0 ? 1 : 0);

            if (GB.ms_to_kmh(TheCarRigidBody.velocity.magnitude) >= generalCar.Speed)
                instantTorque = 0;

            EffettoVelocitaCamera();

            if (!RibaltaDisabilitato)
            {
                var Ribalta = Input.GetKey(KeyCode.T);

                if (Ribalta)
                {
                    RibaltaDisabilitato = true;

                    instantTorque = 0;
                    instantSteeringAngle = 0;

                    var ppp = TheCarRigidBody.gameObject.transform.position;
                    TheCarRigidBody.gameObject.transform.SetPositionAndRotation(new Vector3(ppp.x, 0, ppp.z), OriginalRotation);

                    StartCoroutine(AbilitaRibalta());
                }
            }
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
                    Colliders[i].steerAngle = instantSteeringAngle;

                switch (Trazione)
                {
                    case eTrazione.anteriore:
                        if (Colliders[i].tag.Equals("FrontWheel"))
                            Colliders[i].motorTorque = instantTorque * generalCar.Accellerazione;
                        break;

                    case eTrazione.posteriore:
                        if (Colliders[i].tag.Equals("BackWheel"))
                            Colliders[i].motorTorque = instantTorque * generalCar.Accellerazione;
                        break;
                }

                if (fullBrake > 0)
                {
                    Colliders[i].brakeTorque = fullBrake * generalCar.Accellerazione;
                }
                else if (handBrake > 0)
                {
                    if (Colliders[i].tag.Equals("BackWheel"))
                        Colliders[i].brakeTorque = handBrake * generalCar.Accellerazione;
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
            sandParticle.playbackSpeed = (generalCar.transform.position.y < PosizionePavimento ? generalCar.actualSpeed / 10 : 0);

            var RuoteCheCollidono = 0;
            for (var j = 0; j < Colliders.Length; j++)
                if (Colliders[j].isGrounded)
                    RuoteCheCollidono++;

            //quando sei in aria usa il centro di massa al centro del box 3d, altrimenti usa il centro di massa che Michele ha settato a mano per ogni auto            
            TheCarRigidBody.centerOfMass = (RuoteCheCollidono == 0 ? CentroDiMassa3D : CentroDiMassaAssettoCorsa);

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