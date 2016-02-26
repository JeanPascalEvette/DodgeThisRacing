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
    private Vector3 currentCenterOfGravity = new Vector3(0.2f, 0.5f, 0);


    //

    public float currentGear;
	public float gearOne = 2.66f;		// gears should be applied to the equation to get from engine torque to drive force (Fdrive = u * Tengine * gear *xd * transmission efficiency/wheel radius)
	public float gearTwo = 1.78f;		// however I will apply it to the traction force that we currently have
	public float gearThree = 1.3f;
	public float gearFour = 1.0f;
	public float gearFive = 0.74f;
	public float gearSix = 0.5f;
	public float reverse = 2.9f;

	// public float throttlePosition = 0;   This will not be needed since when user hits key we assume that the throttle pedal is full down.
	public float rpm;

	public float differentialRatio = 3.42f;     // for off road performance we should increase this parameter (like to 4.10f)
                                               
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


    public float speedForStefanos;	// ERASE THIS SOON JUST FOR NOW


    // Use this for initialization
    void Start () {

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
        Gizmos.DrawSphere(transform.position + currentCenterOfGravity, 0.1f);

    }

    // Update is called once per frame
    void Update ()
    {
        //Update CoG
        currentCenterOfGravity = transform.rotation * CenterOfGravity;

        if (!IsOnGround()) return;
        direction = 0.0f;						//speed of object
        float maxTurn = turning * Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.W))
            direction = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            direction = -1.0f;

        if (direction >= 0)														// the traction force is the force delivered by the engine via the rear wheels
        {
            TractionForce = transform.forward * direction * mEngineForce;		// when braking this should be replaced by a braking force
            
        }		// Ftraction = u * Enginforce									// which is oriented in the opposite direction.
        else
        {
            TractionForce = transform.forward * -mCBrake;
        }

        if(transform.InverseTransformDirection(rb.velocity).z < 0)
        {
            gameObject.transform.Rotate(new Vector3(0, maxTurn, 0));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, -maxTurn, 0));
        }

        if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow))
        {
            frontLeftWheel.transform.rotation = frontRightWheel.transform.rotation = gameObject.transform.rotation * Quaternion.AngleAxis(maxTurn * 30, new Vector3(0, 1, 0));
        }
        else
        {
            frontLeftWheel.transform.rotation = frontRightWheel.transform.rotation = gameObject.transform.rotation;
        }
        var speed = Mathf.Sqrt(TractionForce.x * TractionForce.x + TractionForce.z * TractionForce.z);
        DragForce = new Vector3(-mCDrag * TractionForce.x * speed, 0, -mCDrag * TractionForce.z * speed);
        speedForStefanos = speed;// ERASE THIS SOON JUST FOR NOW																									// fdrag.y = Cdrag * v.y * speed
        RollingResistance = -mCRolRes * TractionForce;


        LongtitudinalForce = TractionForce + DragForce + RollingResistance; //Flong = Ftraction + Fdrag + Frr
        Acceleration = LongtitudinalForce / rb.mass;                    // a = F + M

        rb.velocity = rb.velocity + Time.deltaTime * Acceleration;          // v = v + dt * a

        // Dampening X element in local velocity
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x *= 0.5f;
        rb.velocity = transform.TransformDirection(localVelocity);


        rpm = speed * currentGear * differentialRatio * 60.0f / 6.28f;		// 6.28 occurs from 2pi . Correct?

        WeightOnFrontWheels = GetMassOnAxle(frontRightPosition) - (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.magnitude;
        WeightOnRearWheels = GetMassOnAxle(rearRightPosition) + (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.magnitude;

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

    }

    public float GetMassOnAxle(float zCoord)
    {
        float distance = Mathf.Abs(zCoord - currentCenterOfGravity.z);
        float wheelDist = rearRightPosition - frontRightPosition;
        return (distance / wheelDist) * rb.mass;
    }

    private bool IsOnGround()
    {
        return rearRightWheel.isOnGround || rearLeftWheel.isOnGround || frontLeftWheel.isOnGround || frontRightWheel.isOnGround;
    }
}


// car's position -- p = p + dt * v
// torque is equal to force * distance (if you apply a 10Newton force at 0.3 meters of the axis of rotation, the torque = 10 * 0.3 = 3 N.m)
// hp = torque * rpm/5252

// the gearing multiplies the torque from the engine by a factor depending on the gear ratios

/*    
Fdrive = u * Tengine * xg * xd * n / Rw 
	where 
u is a unit vector which reflects the car's orientation, 
Tengine is the torque of the engine at a given rpm,
xg is the gear ratio,
xd is the differential ratio,
n is transmission efficiency and 
Rw is wheel radius.  
*/