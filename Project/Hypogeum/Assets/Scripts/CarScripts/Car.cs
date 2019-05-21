/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

//TODO create a camera class
public class Car : MonoBehaviour
{
    private float maxSteeringAngle = 50f;
    private float maxTorque = 1000f;
    private float brakingTorque = 60000f;

    private float maxSpeed = 20f;

    [Tooltip("m/s")]
    private float speedThreshold = 20f;
    private int stepsBelowThreshold = 30;
    private int stepsAboveThreshold = 1;

    private GameObject wheels;
    private WheelCollider[] wheelColliders;

    private CameraManager MyCamera;
    private Transform LookHere, Position, AimPosition;
    private Rigidbody rb;
    private int speed = 7;
    private int agility = 6;

    public int Health { get; set; } = 1000;
    public int Defense { get; set; } = 7;

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
        MyCamera.AimPosition = AimPosition;
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

        MyCamera = Camera.main.GetComponent<CameraManager>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");
        AimPosition = transform.Find("CameraAnchor/AimPosition");

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


        var fullBrake = (Input.GetKey(KeyCode.M) ? brakingTorque : 0);
        var handBrake = (Input.GetKey(KeyCode.K) ? brakingTorque * 2 : 0);

        foreach (var wheel in wheelColliders)
        {
			if (wheel.transform.localPosition.z > 0)
			{
				wheel.steerAngle = instantSteeringAngle;
				//wheel.motorTorque = instantTorque;
			}

			if (fullBrake > 0)
				wheel.brakeTorque = fullBrake;

			else if (handBrake > 0)
			{
				if (wheel.tag == "BackWheel")
				{
					wheel.brakeTorque = handBrake;
					//wheel.motorTorque = 0;
				}
			}

			else
				wheel.brakeTorque = 0;

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