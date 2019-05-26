using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralCar : MonoBehaviour
{
	private float maxSteeringAngle;
	private float maxTorque;
	private float brakingTorque;

	private float maxSpeed;

	[Tooltip("m/s")]
	private float speedThreshold;
	private int stepsBelowThreshold;
	private int stepsAboveThreshold;

	private GameObject wheels;
	private WheelCollider[] wheelColliders;

	private CameraManager MyCamera;
	private Transform LookHere, Position, AimPosition;
	private Rigidbody rb;

	public abstract void SetCamera();

	public abstract void SetInGameStats();

	public abstract void Drive();

	public abstract void SetWheels();

	public abstract void SetCar();
}
