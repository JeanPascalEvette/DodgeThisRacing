using UnityEngine;
using System.Collections;

public class CarProperties : MonoBehaviour {

    private GameObject carBody;
    public float carSpeed = 50.0f;
    public float carRotation = 20.0f;

    public float springSpring;
    public float springDamper;

    public float carBodyMass;
    public float wheelsMass;

    // Use this for initialization
    void Start () {
        carBody = this.transform.Find("Body").gameObject;

        var joints = carBody.GetComponents<HingeJoint>();
        

        carBody.GetComponent<Rigidbody>().mass = carBodyMass;




        var springs = carBody.GetComponents<SpringJoint>();
        foreach(SpringJoint spring in springs)
        {
            spring.spring = 1000;
            spring.minDistance= 2;
            spring.maxDistance= 3;
            spring.damper= 2;
            spring.tolerance = 0.05f;
        }

    }
	
	// Update is called once per frame
	void Update () {
    }
}
