using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{


    //These variables might need some tuning
    //    [SerializeField]
    // private float mEngineForce = 10.0f;
    [SerializeField]
    private float mCDrag = 0.4257f;
    [SerializeField]
    private float mCRolRes = 12.8f;
    [SerializeField]
    private float mCBrake = 3.0f;
    [SerializeField]
    private Vector3 CenterOfGravity = new Vector3(0.2f, 0.5f, 0);


    //

    public float currentGear;
    private float gearOne = 2.66f;      // gears should be applied to the equation to get from engine torque to drive force (Fdrive = u * Tengine * gear *xd * transmission efficiency/wheel radius)
    private float gearTwo = 1.78f;      // however I will apply it to the traction force that we currently have
    private float gearThree = 1.3f;
    private float gearFour = 1.0f;
    private float gearFive = 0.74f;
    private float gearSix = 0.5f;
    private float reverse = 2.9f;

    public float newVehicleSpeed;
    public float rpm;
    private float differentialRatio = 3.42f;     // for off road performance we should increase this parameter (like to 4.10f)         
    public float rpmToTorque; // This is needed to measure the Tengine which is used in the final formula
    private bool isPedalDown = false;	// checks to see if pedal is down in order to set a minimum rpm of 1000


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

    public float turning = 1;

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
    void Start()
    {

        currentGear = gearOne; 			// bound to change in future // still in testing phase
        rb = GetComponent<Rigidbody>();

        // We set the boolean variables of the wheels
        rearLeftWheel.isRearWheel = true;
        rearRightWheel.isRearWheel = true;
        frontLeftWheel.isRearWheel = false;
        frontRightWheel.isRearWheel = false;

        // We obtain the position of the wheels to calculate the different weights
        frontRightPosition = frontRightWheel.transform.localPosition.z;
        rearRightPosition = rearRightWheel.transform.localPosition.z;
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + CenterOfGravity, 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOnGround()) return;
        direction = 0.0f;
        float maxTurn = turning * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.W))
        {
            direction = 1.0f;
            isPedalDown = true;
            if (isPedalDown == true && rpm < 1000.0f)
            {       // checks that car isn't moving so that rpm can have a minimum of 1000 when it starts movign from inactivity
                rpm = 1000.0f;
                isPedalDown = false;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = -1.0f;
        }

        if (direction >= 0)
        {
            if (rpm < 6000)
            {                                               // car has a maximum traction force when in gear six and has an rpm of 6000
                TractionForce = transform.forward * direction * (rpmToTorque * gearOne * differentialRatio * 0.7f * 0.34f) * 0.5f;
            }
            else
            {
                TractionForce = transform.forward * direction;
            }
        }
        else
        {
            if (rpm < 5500)
            {                                           //set a maximum speed that vehicle will reverse
                TractionForce = transform.forward * -mCBrake * 100;
            }
            else
            {
                TractionForce = transform.forward * direction;
            }
        }

        if (transform.InverseTransformDirection(rb.velocity).z < 0)
        {
            gameObject.transform.Rotate(new Vector3(0, maxTurn, 0));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, -maxTurn, 0));
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            frontLeftWheel.transform.rotation = frontRightWheel.transform.rotation = gameObject.transform.rotation * Quaternion.AngleAxis(maxTurn * 30, new Vector3(0, 1, 0));
        }
        else
        {
            frontLeftWheel.transform.rotation = frontRightWheel.transform.rotation = gameObject.transform.rotation;
        }
        var speed = Mathf.Sqrt(TractionForce.x * TractionForce.x + TractionForce.z * TractionForce.z);
        DragForce = new Vector3(-mCDrag * TractionForce.x * speed, 0, -mCDrag * TractionForce.z * speed);																								// fdrag.y = Cdrag * v.y * speed
        RollingResistance = -mCRolRes * TractionForce;


        LongtitudinalForce = TractionForce + DragForce + RollingResistance; //Flong = Ftraction + Fdrag + Frr
        Acceleration = LongtitudinalForce / rb.mass;                    // a = F + M

        rb.velocity = rb.velocity + Time.deltaTime * Acceleration;          // v = v + dt * a

        // Dampening X element in local velocity
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x *= 0.5f;
        rb.velocity = transform.TransformDirection(localVelocity);


        WeightOnFrontWheels = GetMassOnAxle(frontRightPosition) - (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.x;
        WeightOnRearWheels = GetMassOnAxle(rearRightPosition) + (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.x;

        // We set the mass of each wheels
        frontLeftWheel.wheelMass = WeightOnFrontWheels / 2;
        frontRightWheel.wheelMass = WeightOnFrontWheels / 2;
        rearLeftWheel.wheelMass = WeightOnRearWheels / 2;
        rearRightWheel.wheelMass = WeightOnRearWheels / 2;

        frontLeftWheel.tyreLoad = WeightOnFrontWheels / 2;
        frontRightWheel.tyreLoad = WeightOnFrontWheels / 2;
        rearLeftWheel.tyreLoad = WeightOnRearWheels / 2;
        rearRightWheel.tyreLoad = WeightOnRearWheels / 2;

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


        // **************************** RPM MEASUREMENTS ****************************** //
        newVehicleSpeed = rb.velocity.magnitude;        // used as helper to measure exact speed vehicle is moving 

        if (speed > 0)
        {
            rpm = (newVehicleSpeed * 2.5f) * currentGear * differentialRatio * 60.0f / 6.28f;           //rpm measurement
        }

        if (rpm >= 1000.0f && rpm < 5000.0f)
        {
            rpmToTorque = ((rpm - 1000.0f) * 0.012f) + 300.0f;                                          // rpm converter to torque from 1000- 5000 rpm
        }


        if (rpm >= 5000.0f && rpm <= 6000.0f)
        {                                                           // rpm converter to torque from 5000-6000 rpm
            rpmToTorque = 300.0f - (rpm * 0.05f) + 300.0f;
        }

        if (rpm < 1000.0f && speed != 0)
        {                                                               // when rpm gets below 1000 gear is decreased
            decreaseGear();
        }


        if (rpm > 6000.0f)
        {                                                                           // when rpm gets above 6000 gear is increased
            increaseGear();
        }

        // **************************** END RPM MEASUREMENTS ****************************** //

    }

    public float GetMassOnAxle(float zCoord)
    {
        float distance = Mathf.Abs(zCoord - CenterOfGravity.z);
        float wheelDist = rearRightPosition - frontRightPosition;
        return (distance / wheelDist) * rb.mass;
    }

    private bool IsOnGround()
    {
        return rearRightWheel.isOnGround || rearLeftWheel.isOnGround || frontLeftWheel.isOnGround || frontRightWheel.isOnGround;
    }

    public void increaseGear()
    {

        if (currentGear == gearOne)
        {
            rpm = 1000;
            currentGear = gearTwo;
        }

        else if (currentGear == gearTwo)
        {
            rpm = 1000;
            currentGear = gearThree;
        }

        else if (currentGear == gearThree)
        {
            rpm = 1000;
            currentGear = gearFour;
        }

        else if (currentGear == gearFour)
        {
            rpm = 1000;
            currentGear = gearFive;
        }

        else if (currentGear == gearFive)
        {
            rpm = 1000;
            currentGear = gearSix;
        }

    }

    public void decreaseGear()
    {
        if (currentGear == gearSix)
        {
            currentGear = gearFive;
            rpm = 6000;
        }

        else if (currentGear == gearFive)
        {
            currentGear = gearFour;
            rpm = 6000;
        }

        else if (currentGear == gearFour)
        {
            currentGear = gearThree;
            rpm = 6000;
        }

        else if (currentGear == gearThree)
        {
            currentGear = gearTwo;
            rpm = 6000;
        }

        else if (currentGear == gearTwo)
        {
            currentGear = gearOne;
            rpm = 6000;
        }
    }
}