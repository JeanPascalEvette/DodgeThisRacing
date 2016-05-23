using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public Slider healthSlider;
    private float damageCaused;
    public float pieceHealth;
    public float pieceMass;

    public AnimationCurve slipCurve;
    public AnimationCurve longtitudinalForceCurve;


    // Object to call information of Car Controller Object
    private CarController mCarController;

    private float wheelRadius;
    private float spinningSpeed = 10;

    public Vector3 currentRotation = new Vector3(0, 0, 0);

    private Transform direction;

    // Use this for initialization
    void Start () {
        mCarController = transform.root.GetComponent<CarController>();
        var meshRenderer = transform.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = transform.GetChild(2).GetComponent<MeshRenderer>();
        wheelRadius = meshRenderer.bounds.size.y / 2;
        if (carModel == null)
            carModel = transform.root.GetComponent<Rigidbody>();
        if (gameObject.name == "LeftRear" && transform.root.Find("FrontAxle") != null)
            direction = transform.root.Find("FrontAxle").GetChild(0);
        else if (gameObject.name == "RightRear" && transform.root.Find("FrontAxle") != null)
            direction = transform.root.Find("FrontAxle").GetChild(1);
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

    //Animation of wheels
    void LateUpdate()
    {
        currentRotation.x += spinningSpeed;

        float maxTurn = 0;

        if (mCarController.IsGoing('A'))
        {
            maxTurn = -1;
        }
        else if (mCarController.IsGoing('D'))
        {
            maxTurn = 1;
        }

        float angle = 0f;

        if (direction != null)
            direction.gameObject.transform.localRotation = Quaternion.Euler(0, maxTurn * 30, 0);
        else
            angle = maxTurn * 30;


        if (isRearWheel)
            transform.localRotation = Quaternion.Euler(currentRotation);
        else
            transform.localRotation = Quaternion.Euler(currentRotation.x, angle, 0);
    }

    // Fixed Update Function
    void FixedUpdate()
    {


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
        //*********************** END TRACTION OR LONGTITUDINAL FORCE ***********************//

        //*********************** TRACTION TORQUE ON DRIVE WHEELS ***********************//

        // Traction Torque
        tractionTorque = longtitudinalForce * wheelRadius;
        // We calculate the sign of this force as it has to be opposite to the drive torque
        float signTractionTorque = Mathf.Sign(driveTorque);
        tractionTorque = (-1) * signTractionTorque * tractionTorque;
        //*********************** END TRACTION TORQUE ON DRIVE WHEELS ***********************//
    }
    

    //This uses raytracing to make sure that the wheels are hiting (or close to) the ground or any surface (except for the car itself)
    private void CheckWheelsAreOnGround()
    {
        isOnGround = false;
        Color myColor = Color.red;
        float wheelHeight = transform.GetComponent<SphereCollider>().radius * transform.lossyScale.y * 1.25f;
        Vector3 direction = new Vector3(0, -1, 0);

        int numRaycast = 10;
        Ray myRay;
        for (int i = 0; i < 360/numRaycast; i++)
        {
            direction = Quaternion.Euler(i*360/numRaycast, 0, 0) * direction;
            direction = transform.root.rotation * direction;
            myRay = new Ray(transform.position, direction.normalized);
            if (Physics.Raycast(myRay, wheelHeight, ~(1 << LayerMask.NameToLayer("CarCollisionHitbox"))))
            {
                isOnGround = true;
                myColor = Color.green;
            }
            Debug.DrawLine(transform.position, transform.position + direction.normalized * wheelHeight, myColor);
            myColor = Color.red;
            direction = new Vector3(0, -1, 0);
        }
        
    }
}
