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

    public int currentGear;  
    private float[] gears = { 2.9f, 1.20f, 0.92f, 0.85f, 0.83f, 0.80f, 0.78f }; //0 = Reverse
	/*private float gearOne = 2.66f;      // gears should be applied to the equation to get from engine torque to drive force (Fdrive = u * Tengine * gear *xd * transmission efficiency/wheel radius)
    private float gearTwo = 1.78f;      // however I will apply it to the traction force that we currently have
    private float gearThree = 1.3f;
    private float gearFour = 1.0f;
    private float gearFive = 0.74f;
    private float gearSix = 0.5f;
    private float reverse = 2.9f;
    */
    public float newVehicleSpeed;
	public float rpm;
    private float differentialRatio = 3.42f;     // for off road performance we should increase this parameter (like to 4.10f)         
    public float rpmToTorque; // This is needed to measure the Tengine which is used in the final formula
    private bool isPedalDown = false;   // checks to see if pedal is down in order to set a minimum rpm of 1000
    // Animation Curve for the RPM Torque in which an engine best operated
    public AnimationCurve rpmTorqueCurve;
    public float engineTorque;

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

    [SerializeField]
    private AnimationCurve testRpmResistance;


    // Use this for initialization
    void Start () {

		currentGear = 1; 			// bound to change in future // still in testing phase
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
    void FixedUpdate ()
    {
        //Update CoG
        currentCenterOfGravity = transform.rotation * CenterOfGravity;

        if (!IsOnGround()) return;
        direction = 0.0f;						//speed of object
        float maxTurn = turning * Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = 1.0f;
            isPedalDown = true;
            if (isPedalDown == true && rpm < 1000.0f)
            {       // checks that car isn't moving so that rpm can have a minimum of 1000 when it starts movign from inactivity
                rpm = 1000.0f;
                isPedalDown = false;
        }
        }
           
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // We apply a force in the different direction as it was
            direction = -1.0f;
        }
            

        if (direction >= 0)														// the traction force is the force delivered by the engine via the rear wheels
        {
            if (rpm < 6000)
            {                                               // car has a maximum traction force when in gear six and has an rpm of 6000
                TractionForce = transform.InverseTransformDirection(transform.forward) * direction * (rpmToTorque * gears[currentGear] * differentialRatio * 0.7f * 0.34f);// * testRpmResistance.Evaluate((float)currentGear/6);
            }
            else
            {
                TractionForce = transform.InverseTransformDirection(transform.forward) * direction;
            }
        }		// Ftraction = u * Enginforce									// which is oriented in the opposite direction.
        else
        {
            if (rpm < 5500)
            {                                           //set a maximum speed that vehicle will reverse
            TractionForce = transform.InverseTransformDirection(transform.forward) * -mCBrake;
        }
            else
            {
                TractionForce = transform.InverseTransformDirection(transform.forward) * direction;
            }
        }

        if (transform.InverseTransformDirection(rb.velocity).magnitude > 0.1f)
        {
            if (transform.InverseTransformDirection(rb.velocity).z < 0)
        {
            gameObject.transform.Rotate(new Vector3(0, maxTurn, 0));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, -maxTurn, 0));
        }
        }

        
        var speed = Mathf.Sqrt(TractionForce.x * TractionForce.x + TractionForce.z * TractionForce.z);
        DragForce = new Vector3(-mCDrag * TractionForce.x * speed, 0, -mCDrag * TractionForce.z * speed);
        RollingResistance = -mCRolRes * TractionForce;


        LongtitudinalForce = TractionForce + DragForce + RollingResistance; //Flong = Ftraction + Fdrag + Frr
        Acceleration = LongtitudinalForce / rb.mass;                    // a = F + M

        rb.velocity = rb.velocity + Time.deltaTime * transform.TransformDirection(Acceleration);          // v = v + dt * a

        if (Acceleration == new Vector3(0, 0, 0))
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
                rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
            if (Mathf.Abs(rb.velocity.z) < 0.1f)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        }
        // Dampening X element in local velocity
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x *= 0.5f;
        rb.velocity = transform.TransformDirection(localVelocity);

        //*********************** RPM CALCULATION ***********************//
        //rpm = speed * currentGear * differentialRatio * 60.0f / 6.28f;		// 6.28 occurs from 2pi . Correct?
        // Clamp the rpm between its min and max
        //rpm = Mathf.Clamp(rpm, 1000, rpmMax);

        // We calculate the Engine Torque that will end up in the rear Wheels as Drive Torque (RPM is needed)
        // We use the curve of the Torque RPM in the page
        //float maxEngineTorque = rpmTorqueCurve.Evaluate(rpm / rpmMax) * mEngineForce;

        // **************************** RPM MEASUREMENTS ****************************** //
        newVehicleSpeed = rb.velocity.magnitude;        // used as helper to measure exact speed vehicle is moving 

        if (speed > 0)
        {
            rpm = (newVehicleSpeed * 2.5f) * gears[currentGear] * differentialRatio * 60.0f / 6.28f;           //rpm measurement
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

        float maxEngineTorque = rpmToTorque;

        // **************************** END RPM MEASUREMENTS ****************************** //

        // The engine Torque is going to depend on the Throttle
        engineTorque = Input.GetAxis("Vertical") * maxEngineTorque;

        // We send this engineTorque to the rear wheels only
        rearLeftWheel.driveTorque = engineTorque;
        rearRightWheel.driveTorque = engineTorque;
        //*********************** END OF RPM CALCULATION ***********************//

        //*********************** WEIGTH CALCULATION ***********************//
        // We calculate the weights on each axle to see the tyre load of each wheel
        WeightOnFrontWheels = GetMassOnAxle(frontRightPosition) - (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.magnitude;
        WeightOnRearWheels = GetMassOnAxle(rearRightPosition) + (CenterOfGravity.y / (frontRightPosition - rearRightPosition)) * rb.mass * Acceleration.magnitude;

        // We set the load on each tyre depending on the weight
        frontLeftWheel.tyreLoad = WeightOnFrontWheels / 2;
        frontRightWheel.tyreLoad = WeightOnFrontWheels / 2;
        rearLeftWheel.tyreLoad = WeightOnRearWheels / 2;
        rearRightWheel.tyreLoad = WeightOnRearWheels / 2;



        //*********************** TORQUE REAR AXLE ***********************//

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

    public void increaseGear()
    {
        if (currentGear >= 6) return;
        currentGear++;
        rpm = 1000;
    }

    public void decreaseGear()
    {
        if (currentGear <= 1) return;
        currentGear--;
        rpm = 6000;
    }
    public float GearValue()
    {
        return gears[currentGear];
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