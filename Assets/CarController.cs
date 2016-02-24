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

	public float differentialRatio = 3.42f;		// for off road performance we should increase this parameter (like to 4.10f)
	//

    private float direction = 0.0f;


    private Rigidbody rb;

    [SerializeField]
    private Vector3 tractionForce;
    [SerializeField]
    private Vector3 dragForce;
    [SerializeField]
    private Vector3 rollingResistance;
    [SerializeField]
    private Vector3 longtitudinalForce;


	public float speedForStefanos;	// ERASE THIS SOON JUST FOR NOW


    // Use this for initialization
    void Start () {

		currentGear = gearOne; 			// bound to change in future // still in testing phase
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        direction = 0.0f;						//speed of object
        if (Input.GetKey(KeyCode.W))
            direction = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            direction = -1.0f;
        if (direction >= 0)														// the traction force is the force delivered by the engine via the rear wheels
            tractionForce = transform.forward * direction * mEngineForce;		// when braking this should be replaced by a braking force
				// Ftraction = u * Enginforce									// which is oriented in the opposite direction.
        else
            tractionForce = transform.forward * -mCBrake;
		var speed = Mathf.Sqrt(tractionForce.x * tractionForce.x + tractionForce.y * tractionForce.y);		// speed = sqrt(v.x * v.x + v.y * v.y)
        dragForce = new Vector3(-mCDrag * tractionForce.x * speed, -mCDrag * tractionForce.y * speed, 0);	// fdrag.x = Cdrag *v.x * speed
		speedForStefanos = speed;// ERASE THIS SOON JUST FOR NOW																									// fdrag.y = Cdrag * v.y * speed
        rollingResistance = -mCRolRes * tractionForce;							// the resistance from the wheels


		longtitudinalForce = tractionForce + dragForce + rollingResistance; //Flong = Ftraction + Fdrag + Frr
		var acceleration = longtitudinalForce / rb.mass;					// a = F + M

		rb.velocity = rb.velocity + Time.deltaTime * acceleration;			// v = v + dt * a



		rpm = speed * currentGear * differentialRatio * 60.0f / 6.28f;		// 6.28 occurs from 2pi . Correct?

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