/*
Copyright (c) 2018 Unity Technologies ApS
Author: Unity Technologies ApS
Contributors: Maione Michele, Carrarini Andrea
Unity Technologies ApS (“Unity”, “our” or “we”) provides game-development and related software (the “Software”), development-related services (like Unity Teams (“Developer Services”)), and various Unity communities (like Unity Answers and Unity Connect (“Communities”)), provided through or in connection with our website, accessible at unity3d.com or unity.com (collectively, the “Site”). Except to the extent you and Unity have executed a separate agreement, these terms and conditions exclusively govern your access to and use of the Software, Developer Services, Communities and Site (collectively, the “Services”), and constitute a binding legal agreement between you and Unity (the “Terms”).
If you accept or agree to the Agreement on behalf of a company, organization or other legal entity (a “Legal Entity”), you represent and warrant that you have the authority to bind that Legal Entity to the Agreement and, in such event, “you” and “your” will refer and apply to that company or other legal entity.
You acknowledge and agree that, by accessing, purchasing or using the services, you are indicating that you have read, understand and agree to be bound by the agreement whether or not you have created a unity account, subscribed to the unity newsletter or otherwise registered with the site. If you do not agree to these terms and all applicable additional terms, then you have no right to access or use any of the services.
*/
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralCar : MonoBehaviour
{

    private int? Health_c;
    protected abstract int Health_default();
    internal int Health { get => (Health_c.HasValue ? Health_c.Value : Health_default()); set => Health_c = value; }

    protected abstract int Defense_default();
    internal int Defense => Defense_default();

    protected abstract int Speed_default();
    internal int Speed => Speed_default();

    protected abstract int Agility_default();
    internal int Agility => Agility_default();

    protected abstract float maxSteeringAngle_default();
    internal float maxSteeringAngle => maxSteeringAngle_default();

    protected abstract float maxTorque_default();
    internal float maxTorque => maxTorque_default();

    protected abstract float brakingTorque_default();
    internal float brakingTorque => brakingTorque_default();

    protected abstract float maxSpeed_default();
    internal float maxSpeed => maxSpeed_default();


    [Tooltip("m/s")]
    protected float speedThreshold = 20f;
    protected int stepsBelowThreshold = 30;
    protected int stepsAboveThreshold = 1;

    private Dictionary<WheelCollider, GameObject> wheelsAndColliders = new Dictionary<WheelCollider, GameObject>();

    protected CameraManager MyCamera;
    protected Transform LookHere, Position, AimPosition;
    protected Rigidbody rb;


    void Start()
    {
        SetWheels();
    }

    protected void SetInGameStats()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void SetWheels()
    {
        if (wheelsAndColliders.Count == 0)
        {
            var wc = GetComponentsInChildren<WheelCollider>();

            foreach (var w in wc)
            {
                var obj = GameObject.Find($"Mesh/Wheel_{w.name}");
                wheelsAndColliders.Add(w, obj);
            }
        }
    }

    protected void SetCamera()
    {
        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
        MyCamera.AimPosition = AimPosition;
    }

    internal void SetCar()
    {
        SetWheels();

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

        foreach (var w in wheelsAndColliders)
        {
            //only 1
            w.Key.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
            break;
        }

        MyCamera = Camera.main.GetComponent<CameraManager>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        AimPosition = transform.Find("CameraAnchor/AimPosition");

        SetCamera();
        SetInGameStats();
    }

    internal void Drive()
    {
        Quaternion q;
        Vector3 p;

        var instantSteeringAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
        var instantTorque = maxTorque * Input.GetAxis("Vertical");

        // limiting speed to maxSpeed 
        // not precise, speed can overcome the limit
        // it only stops motor torque when it's above maxSpeed
        if (rb.velocity.magnitude >= maxSpeed)
            instantTorque = 0f;

        var fullBrake = (Input.GetKey(KeyCode.M) ? brakingTorque : 0);
        var handBrake = (Input.GetKey(KeyCode.K) ? brakingTorque * 2 : 0);

        foreach (var wheel in wheelsAndColliders)
        {
            if (wheel.Key.transform.localPosition.z > 0)
                wheel.Key.steerAngle = instantSteeringAngle;

            if (fullBrake > 0)
            {
                wheel.Key.brakeTorque = fullBrake;
            }
            else if (handBrake > 0)
            {
                if (wheel.Key.tag == "BackWheel")
                    wheel.Key.brakeTorque = handBrake;
            }
            else
            {
                wheel.Key.brakeTorque = 0;
            }

            wheel.Key.motorTorque = instantTorque;

            wheel.Key.GetWorldPose(out p, out q);

            //rotate the 3d object            
            wheel.Value.transform.position = p;
            wheel.Value.transform.rotation = q;
        }
    }


}