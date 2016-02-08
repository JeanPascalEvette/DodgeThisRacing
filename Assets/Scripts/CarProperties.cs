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
        
    }
	
	// Update is called once per frame
	void Update () {

	}
}
