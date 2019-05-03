using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//TODO create a camera class
public class Car : MonoBehaviour
{
    private float maxSteeringAngle = 30f;
    private float maxTorque = 1000f;
    private float brakingTorque = 30000f;

    private float maxSpeed = 20f;

    [Tooltip("m/s")]
    private float speedThreshold = 5f;
    private int stepsBelowThreshold = 5;
    private int stepsAboveThreshold = 1;

    private GameObject wheels;
    private WheelCollider[] wheelColliders;

    private cCamera MyCamera;
    private Transform LookHere, Position;
    private Rigidbody rb;

    //car in-game stats (Sharks)
    private int health = 1000;
    private int speed = 7;
    private int agility = 6;
    private int defense = 7;

    public int Health { get => health; set => health = value; }
    public int Defense { get => defense; set => defense = value; }

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

    public void SetCamera()
    {
        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
    }

    public void SetInGameStats()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetCar()
    {
        setWheels();
        //     Configure vehicle sub-stepping parameters.
        //
        // Parameters:
        //   speedThreshold:
        //     The speed threshold of the sub-stepping algorithm.
        //
        //   stepsBelowThreshold:
        //     Amount of simulation sub-steps when vehicle's speed is below speedThreshold.
        //
        //   stepsAboveThreshold:
        //     Amount of simulation sub-steps when vehicle's speed is above speedThreshold.
        wheelColliders[0].ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
        MyCamera = Camera.main.GetComponent<cCamera>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        SetCamera();
        SetInGameStats();
    }

    public void Drive()
    {

        var instantSteeringAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
        var instantTorque = maxTorque * Input.GetAxis("Vertical");
        
        //Debug.Log("Speed: " + rb.velocity.magnitude);
        
        // limiting speed to maxSpeed 
        // not precise, speed can overcome the limit
        // it only stops motor torque when it's above maxSpeed
        if (rb.velocity.magnitude >= maxSpeed)
        {
            instantTorque = 0f;
        }

        var handBrake = Input.GetKey(KeyCode.M) ? brakingTorque : 0;

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
