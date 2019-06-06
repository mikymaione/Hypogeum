/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AutoGuida : NetworkBehaviour
{

    [Tooltip("m/s")]
    private float speedThreshold = 20f;
    private int stepsBelowThreshold = 30;
    private int stepsAboveThreshold = 1;

    private Quaternion[] WheelErrorCorrectionR = new Quaternion[4];
    private WheelCollider[] Colliders = new WheelCollider[4];
    private GameObject[] Wheels = new GameObject[4];

    private CameraManager MyCamera;
    private Transform LookHere, Position, AimPosition;
    private Rigidbody TheCarRigidBody;

    //The class that owns the stats of the faction
    public GeneralCar generalCar;
    //To change the speed in the speedometer
    private Text speedText;
    //To manage the team Health bar
    private Slider healthBar;


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

        generalCar = GeneralCar.IstanziaMe();
        TheCarRigidBody = GetComponent<Rigidbody>();
        MyCamera = Camera.main.GetComponent<CameraManager>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        AimPosition = transform.Find("CameraAnchor/AimPosition");
        speedText = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<Text>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        //Setting the start values for Health Bar
        healthBar.minValue = 0;
        healthBar.maxValue = generalCar.Max_Health;
        healthBar.value = generalCar.Health;

        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
        MyCamera.AimPosition = AimPosition;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            Quaternion worldPose_rotation;
            Vector3 worldPose_position;

            //To update the Health Bar
            healthBar.value = generalCar.Health;

            //To display the car speed on HUD
            var actualSpeed = TheCarRigidBody.velocity.magnitude;
            speedText.text = Mathf.Round(actualSpeed).ToString();

            Colliders[0].ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);

            //freni
            var fullBrake = (Input.GetKey(KeyCode.M) ? generalCar.brakingTorque : 0);
            var handBrake = (Input.GetKey(KeyCode.K) ? generalCar.brakingTorque * 2 : 0);

            //DX-SX
            var instantSteeringAngle = generalCar.maxSteeringAngle * Input.GetAxis("Horizontal");

            //Avanti-dietro
            var instantTorque = generalCar.maxTorque * Input.GetAxis("Vertical");

            if (TheCarRigidBody.velocity.magnitude >= generalCar.maxSpeed)
                instantTorque = 0f;

            for (var i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].tag.Equals("FrontWheel"))
                    Colliders[i].steerAngle = instantSteeringAngle;

                if (fullBrake > 0)
                {
                    Colliders[i].brakeTorque = fullBrake;
                }
                else if (handBrake > 0)
                {
                    if (Colliders[i].tag.Equals("BackWheel"))
                        Colliders[i].brakeTorque = handBrake;
                }
                else
                {
                    Colliders[i].brakeTorque = 0;
                }

                Colliders[i].motorTorque = instantTorque;

                //rotate the 3d object
                Colliders[i].GetWorldPose(out worldPose_position, out worldPose_rotation);

                Wheels[i].transform.position = worldPose_position;
                Wheels[i].transform.rotation = worldPose_rotation * WheelErrorCorrectionR[i];
            }
        }
    }


}