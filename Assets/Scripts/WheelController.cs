using UnityEngine;
using System.Collections;

public class WheelController : MonoBehaviour {

    [SerializeField]
    private float mFrictionCoef = 1.0f;

    // Variables of a wheel
    public float wheelMass;
    public bool isRearWheel;
    public bool isOnGround;

    // Variables for Slip Ratio
    public float angularVelocity;
    // Regarding formula linearVelocity = angularVeloctiy * Radius
    public float wheelLinearVelocity;
    public float slipRatio;

    // Traction force variables
    public float tyreLoad;
    public float longtitudinalForce;
    public float normLongForce;
    public float tractionConstant;
    // In case we want to cap the maximum longtitudinal force
    public float maxLongForce;

    // Variables of Torque in Drive Wheels
    public float tractionTorque;
    public float driveTorque;
    public float brakeTorque;

    // Acceleration consequence of all Torque force applied for rear wheels
    public float angularAcceleration;
    public float wheelInertia;

    // General car variables
    public Rigidbody carModel;
    public float carSpeed;

    public AnimationCurve slipCurve;
    public AnimationCurve longtitudinalForceCurve;


    // Object to call information of Car Controller Object
    private CarController mCarController;

    private float wheelRadius;
    private float spinningSpeed = 10;

    private Vector3 currentRotation = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {
        mCarController = transform.parent.GetComponent<CarController>();
        wheelRadius = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.y / 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckWheelsAreOnGround();
    }

    public float GetFrictionLimit()
    {
        return mFrictionCoef * mCarController.GetMassOnAxle(transform.localPosition.z);
    }



    // Fixed Update Function
    void FixedUpdate()
    {
        currentRotation.x += spinningSpeed;
        if (!isRearWheel)
        {
            float maxTurn = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                currentRotation = new Vector3(currentRotation.x, maxTurn * 30, currentRotation.z);
            }
            else
            {
                currentRotation = new Vector3(currentRotation.x, 0, currentRotation.z);
            }
            
        }

        // Car velocity (Find out which direction to use) (negative sign as before the speed was opposite it should be)
        carSpeed = carModel.transform.InverseTransformDirection(carModel.velocity).z;
        spinningSpeed = carSpeed;

        //*********************** WHEEL INERTIA ***********************//
        wheelInertia = (wheelMass * wheelRadius * wheelRadius) / 2;
        //*********************** END WHEEL INERTIA ***********************//

        //*********************** ANGULAR ACCELERATION ***********************//
        // We calculate the total torque to be applied in case we need this for the angular velocity
        float totalTorque = driveTorque + brakeTorque + tractionTorque;
        // We calculate now the angular acceleration to be applied
        angularAcceleration = totalTorque / wheelInertia;        
        //*********************** END ANGULAR ACCELERATION ***********************//

        //*********************** ANGULAR VELOCITY SECTION ***********************//
        // We need to know if the wheel is front or rear as we need to apply some rear wheel acceleration previously calculated
        if (isRearWheel){
            // We add to the angular velocity the angular acceleration * time between each frame
            angularVelocity += angularAcceleration * Time.fixedDeltaTime;
        } else {
            // We are in front wheels and the wheels spin free with angular velocity = car speed / 2 PI Radius
            angularVelocity = carSpeed / (6.28f * wheelRadius);
        }
        // We calculate the lineal velocity of the wheel with the previous angular velocity
        wheelLinearVelocity = angularVelocity * wheelRadius;
        //*********************** END ANGULAR VELOCITY SECTION ***********************//

        //*********************** SLIP RATIO ***********************//

        // Car velocity (Find out which direction to use) (negative sign as before the speed was opposite it should be)
        carSpeed = -carModel.transform.InverseTransformDirection(carModel.velocity).z;
        spinningSpeed = carSpeed;
        // Wheel linear velocity 
        angularVelocity = carSpeed / wheelRadius;

       
        // Slip ratio using the wheel velocity and the car speed
        slipRatio = -(wheelLinearVelocity - carSpeed) / Mathf.Abs(carSpeed);
        wheelLinearVelocity = angularVelocity * wheelRadius;

        // We check particular cases of Slip Ratio
        // 0/0 division
        if (float.IsNaN(slipRatio)) {
            slipRatio = 0.0f;
        }
        // X/0 division
        else if (float.IsInfinity(slipRatio)){
            slipRatio = 1.0f * Mathf.Sign(slipRatio);
        }
        //*********************** END SLIP RATIO ***********************//

        //*********************** TRACTION OR LONGTITUDINAL FORCE ***********************//
        // Longtitudinal force
        longtitudinalForce = tractionConstant * slipRatio;
        // Normalized Longtitudinal Force
        normLongForce = longtitudinalForce / tyreLoad;
        // We create a vector to apply the force into the car
        //Vector3 tractionVector = carModel.transform.forward * normLongForce;
        // We should apply this force to the car itself
        //carModel.AddForceAtPosition(tractionVector, carModel.position);
        //*********************** END TRACTION OR LONGTITUDINAL FORCE ***********************//

        //*********************** TRACTION TORQUE ON DRIVE WHEELS ***********************//

        // Traction Torque
        tractionTorque = longtitudinalForce * wheelRadius;
        // We calculate the sign of this force as it has to be opposite to the drive torque
        float signTractionTorque = Mathf.Sign(driveTorque);
        tractionTorque = (-1) * signTractionTorque * tractionTorque;
        //*********************** END TRACTION TORQUE ON DRIVE WHEELS ***********************//
    }

    private void CheckWheelsAreOnGround()
    {
        float wheelHeight = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.y;
        Vector3 direction = new Vector3(0, -wheelHeight / 1.999f, 0);
        direction = GetComponent<Collider>().transform.root.rotation * direction;
        Debug.DrawLine(transform.position, transform.position + direction, Color.green);
        Ray myRay = new Ray(transform.position, direction);
        if (Physics.Raycast(myRay, wheelHeight / 1.9f))
            isOnGround = true;
        else
            isOnGround = false;
    }
}
