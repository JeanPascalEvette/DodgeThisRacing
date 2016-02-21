using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {


    //These variables might need some tuning
    [SerializeField]
    private float mEngineForce = 10.0f;
    [SerializeField]
    private float mCDrag = 0.4257f;
    [SerializeField]
    private float mCRolRes = 12.8f;
    [SerializeField]
    private float mCBrake = 3.0f;
    [SerializeField]
    private Vector3 CenterOfGravity = new Vector3(0.2f, 0.5f, 0);

    // Wheels declaration
    public WheelController rearLeftWheel;
    public WheelController rearRightWheel;
    public WheelController frontLeftWheel;
    public WheelController frontRightWheel; 

    // Wheels Position Variables
    private float frontRightPosition;
    private float rearRightPosition;


    // Variable to calculate the total amount of Torque in the car
    public float rearAxleTorque;

    private float direction = 0.0f;

    private Rigidbody rb;

    [SerializeField]
    private Vector3 TractionForce;
    [SerializeField]
    private Vector3 DragForce;
    [SerializeField]
    private Vector3 RollingResistance;
    [SerializeField]
    private Vector3 LongtitudinalForce;
    [SerializeField]
    private Vector3 Acceleration;
    [SerializeField]
    private float WeightOnFrontWheels;
    [SerializeField]
    private float WeightOnRearWheels;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        // We find the Wheels
        rearLeftWheel = transform.Find("RearLeftWheel").GetComponent<WheelController>();
        rearRightWheel = transform.Find("RearRightWheel").GetComponent<WheelController>();
        frontLeftWheel = transform.Find("FrontLeftWheel").GetComponent<WheelController>();
        frontRightWheel = transform.Find("FrontRightWheel").GetComponent<WheelController>();

        // We set the boolean variables of the wheels
        rearLeftWheel.isRearWheel = true;
        rearRightWheel.isRearWheel = true;
        frontLeftWheel.isRearWheel = false;
        frontRightWheel.isRearWheel = false;

        // We obtain the position of the wheels to calculate the different weights
        frontRightPosition = transform.Find("FrontRightWheel").localPosition.z;
        rearRightPosition = transform.Find("RearRightWheel").localPosition.z;

    }
	
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position+CenterOfGravity, 0.1f);

    }

    // Update is called once per frame
    void Update ()
    {
        direction = 0.0f;
        if (Input.GetKey(KeyCode.W))
            direction = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            direction = -1.0f;
        if (direction >= 0)
            TractionForce = transform.forward * direction * mEngineForce;
        else
            TractionForce = transform.forward * -mCBrake;
        var speed = Mathf.Sqrt(TractionForce.x * TractionForce.x + TractionForce.z * TractionForce.z);
        DragForce = new Vector3(-mCDrag * TractionForce.x * speed, -mCDrag * TractionForce.z * speed, 0);
        RollingResistance = -mCRolRes * TractionForce;


        LongtitudinalForce = TractionForce + DragForce + RollingResistance;
        Acceleration = LongtitudinalForce / rb.mass;

        rb.velocity = rb.velocity + Time.deltaTime * Acceleration;

        WeightOnFrontWheels = GetMassOnAxle(frontRightPosition) - (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.x;
        WeightOnRearWheels = GetMassOnAxle(rearRightPosition) + (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.x;

        //*********************** TORQUE REAR AXLE ***********************//

        // Net Torque in the REAR AXLE

        // Drive Torque from both wheels or only one
        float driveTorqueTotal = rearLeftWheel.driveTorque + rearRightWheel.driveTorque;
        // Traction Torque from both rear wheels
        float tractionTorqueTotal = rearLeftWheel.tractionTorque + rearRightWheel.tractionTorque;
        // Brake Torque from both rear wheels
        float brakeTorqueTotal = rearLeftWheel.brakeTorque + rearRightWheel.brakeTorque;
        // Total Net Torque
        rearAxleTorque = driveTorqueTotal + tractionTorqueTotal + brakeTorqueTotal;

        //*********************** END TORQUE REAR AXLE ***********************//

        //*********************** ANGULAR ACCELERATION TO BE APPLIED TO DRIVE WHEELS ***********************//

        // Rear Wheel Inertia
        float rearInertiaTotal = rearLeftWheel.wheelInertia + rearRightWheel.wheelInertia;
        // Angular Acceleration to be applied to rear wheels
        float angularAcceleration = rearAxleTorque / rearInertiaTotal;

        // We apply this force to the rear wheels (Divided by 2 because we split equally the total force between both wheels)
        rearLeftWheel.angularAcceleration = angularAcceleration / 2;
        rearRightWheel.angularAcceleration = angularAcceleration / 2;

        //*********************** END ANGULAR ACCELERATION TO BE APPLIED TO DRIVE WHEELS ***********************//

    }

    public float GetMassOnAxle(float zCoord)
    {
        float distance = Mathf.Abs(zCoord - CenterOfGravity.z);
        float wheelDist = rearRightPosition - frontRightPosition;
        return (distance / wheelDist) * rb.mass;
    }
}
