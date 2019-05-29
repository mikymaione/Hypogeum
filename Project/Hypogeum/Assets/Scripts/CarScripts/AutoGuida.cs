/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoGuida : NetworkBehaviour
{

    [Tooltip("m/s")]
    private float speedThreshold = 20f;
    private int stepsBelowThreshold = 30;
    private int stepsAboveThreshold = 1;

    private Dictionary<WheelCollider, Quaternion> WheelErrorCorrectionR = new Dictionary<WheelCollider, Quaternion>();
    private Dictionary<WheelCollider, GameObject> wheelsAndColliders = new Dictionary<WheelCollider, GameObject>();

    private CameraManager MyCamera;
    private Transform LookHere, Position, AimPosition;
    private Rigidbody TheCarRigidBody;


    // var for rotate the 3d object wheels
    private Quaternion worldPose_rotation;
    private Vector3 worldPose_position;

    private GeneralCar generalCar;


    void Start()
    {
        SetWheels();
    }

    public override void OnStartLocalPlayer()
    {
        foreach (var w in wheelsAndColliders)
        {
            //only 1
            w.Key.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
            break;
        }

        generalCar = GetComponent<GeneralCar>();
        TheCarRigidBody = GetComponent<Rigidbody>();
        MyCamera = Camera.main.GetComponent<CameraManager>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        AimPosition = transform.Find("CameraAnchor/AimPosition");

        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
        MyCamera.AimPosition = AimPosition;
    }

    void Update()
    {
        if (isLocalPlayer)
            Drive();
    }

    private void SetWheels()
    {
        if (wheelsAndColliders.Count == 0)
        {
            var wc = GetComponentsInChildren<WheelCollider>();

            foreach (var w in wc)
            {
                var obj = GameObject.Find($"Mesh/Wheel_{w.name}");

                WheelErrorCorrectionR.Add(w, obj.transform.rotation);
                wheelsAndColliders.Add(w, obj);
            }
        }
    }

    private void Drive()
    {
        var instantSteeringAngle = generalCar.maxSteeringAngle * Input.GetAxis("Horizontal");
        var instantTorque = generalCar.maxTorque * Input.GetAxis("Vertical");

        // limiting speed to maxSpeed 
        // not precise, speed can overcome the limit
        // it only stops motor torque when it's above maxSpeed
        if (TheCarRigidBody.velocity.magnitude >= generalCar.maxSpeed)
            instantTorque = 0f;

        var fullBrake = (Input.GetKey(KeyCode.M) ? generalCar.brakingTorque : 0);
        var handBrake = (Input.GetKey(KeyCode.K) ? generalCar.brakingTorque * 2 : 0);

        foreach (var wheel in wheelsAndColliders)
        {
            if (wheel.Key.tag.Equals("FrontWheel"))
                wheel.Key.steerAngle = instantSteeringAngle;

            if (fullBrake > 0)
            {
                wheel.Key.brakeTorque = fullBrake;
            }
            else if (handBrake > 0)
            {
                if (wheel.Key.tag.Equals("BackWheel"))
                    wheel.Key.brakeTorque = handBrake;
            }
            else
            {
                wheel.Key.brakeTorque = 0;
            }

            wheel.Key.motorTorque = instantTorque;

            //rotate the 3d object
            wheel.Key.GetWorldPose(out worldPose_position, out worldPose_rotation);

            wheel.Value.transform.position = worldPose_position;
            wheel.Value.transform.rotation = worldPose_rotation * WheelErrorCorrectionR[wheel.Key];
        }
    }


}