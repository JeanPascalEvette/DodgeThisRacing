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
    private Transform RearLeftWheel;
    [SerializeField]
    private Transform RearRightWheel;
    [SerializeField]
    private Transform FrontLeftWheel;
    [SerializeField]
    private Transform FrontRightWheel;
    [SerializeField]
    private Vector3 CenterOfGravity = new Vector3(0.2f,0.5f,0);




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


        WeightOnFrontWheels = GetMassOnAxle(FrontRightWheel.localPosition.z) - (CenterOfGravity.y / (FrontRightWheel.localPosition.z - RearRightWheel.localPosition.z)) * rb.mass * Acceleration.x;
        WeightOnRearWheels = GetMassOnAxle(RearRightWheel.localPosition.z) + (CenterOfGravity.y / (FrontRightWheel.localPosition.z - RearRightWheel.localPosition.z)) * rb.mass * Acceleration.x;

    }

    public float GetMassOnAxle(float zCoord)
    {
        float distance = Mathf.Abs(zCoord - CenterOfGravity.z);
        float wheelDist = RearRightWheel.localPosition.z - FrontRightWheel.localPosition.z;
        return (distance / wheelDist) * rb.mass;
    }
}
