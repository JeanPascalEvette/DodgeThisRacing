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

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

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
            tractionForce = transform.forward * direction * mEngineForce;
        else
            tractionForce = transform.forward * -mCBrake;
        var speed = Mathf.Sqrt(tractionForce.x * tractionForce.x + tractionForce.y * tractionForce.y);
        dragForce = new Vector3(-mCDrag * tractionForce.x * speed, -mCDrag * tractionForce.y * speed, 0);
        rollingResistance = -mCRolRes * tractionForce;


        longtitudinalForce = tractionForce + dragForce + rollingResistance;
        var acceleration = longtitudinalForce / rb.mass;

        rb.velocity = rb.velocity + Time.deltaTime * acceleration;

    }
}
