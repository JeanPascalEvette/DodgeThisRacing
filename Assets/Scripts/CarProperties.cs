using UnityEngine;
using System.Collections;

public class CarProperties : MonoBehaviour {

    private GameObject carBody;
    public float carSpeed = 50.0f;
    public float carRotation = 20.0f;

    public float springSpring;
    public float springDamper;
    public float springMinDistance;
    public float springMaxDistance;
    public float springTolerance;
    public float carBodyMass;
    public float wheelsMass;

    // Use this for initialization
    void Start () {
        carBody = this.transform.Find("Body").gameObject;

        var springs = carBody.GetComponents<SpringJoint>();

        foreach(SpringJoint spring in springs)
        {
            spring.spring = springSpring;
            spring.damper = springDamper;
            spring.minDistance = springMinDistance;
            spring.maxDistance= springMaxDistance;
            spring.tolerance = springTolerance;
        }

        carBody.GetComponent<Rigidbody>().mass = carBodyMass;

        var wheelsF = transform.Find("Direction Wheels").GetComponentsInChildren<Rigidbody>();
        var wheelsB = transform.Find("Motion Wheels").GetComponentsInChildren<Rigidbody>();


        foreach (Rigidbody rb in wheelsF)
        {
            rb.mass = wheelsMass;
        }
        foreach (Rigidbody rb in wheelsB)
        {
            rb.mass = wheelsMass;
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
