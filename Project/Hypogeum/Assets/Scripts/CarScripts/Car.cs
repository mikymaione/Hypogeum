using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Car : MonoBehaviour
{
    private float maxSteeringAngle = 30f;
    private float maxTorque = 1000f;
    private float brakingTorque = 30000f;

    [Tooltip("m/s")]
    private float criticSpeed = 5f;
    private int lowerLimit = 5;
    private int upperLimit = 1;

    private GameObject wheels;
    private WheelCollider[] wheelColliders;

    private cCamera MyCamera;
    private Transform LookHere, Position;

    public void setWheels()
    {
        wheelColliders = GetComponentsInChildren<WheelCollider>();

        for (var i = 0; i < wheelColliders.Length; ++i)
        {
            var wheel = wheelColliders[i];

            // Create wheel shapes only when needed.
            if (wheels != null)
            {
                var ws = Instantiate(wheels);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    private void SetCamera()
    {
        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
    }

    public void SetCar()
    {
        setWheels();
        wheelColliders[0].ConfigureVehicleSubsteps(criticSpeed, lowerLimit, upperLimit);
        MyCamera = Camera.main.GetComponent<cCamera>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        SetCamera();
    }

    public void Drive()
    {
        var instantSteeringAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
        var instantTorque = maxTorque * Input.GetAxis("Vertical");

        var handBrake = Input.GetKey(KeyCode.X) ? brakingTorque : 0;

        foreach (var wheel in wheelColliders)
        {
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = instantSteeringAngle;

            if (wheel.transform.localPosition.z < 0)
                wheel.brakeTorque = handBrake;

            wheel.motorTorque = instantTorque;

            if (wheels)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                var t = wheel.transform.GetChild(0);
                t.position = p;
                t.rotation = q;
            }
        }
    }
}
