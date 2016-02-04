using UnityEngine;
using System.Collections;

public class CarProperties : MonoBehaviour {

    private GameObject carBody;
    public float springJoinDistance = 2.0f;
    public float carSpeed = 50.0f;
    public float carRotation = 20.0f;
    public float motorTargetVelocity = 100.0f;
    public float motorForce = 50.0f;
    // Use this for initialization
    void Start () {
        carBody = this.transform.Find("Body").gameObject;
        JointMotor newMotor = new JointMotor();
        newMotor.targetVelocity = -motorTargetVelocity;
        newMotor.force = motorForce;
        
        JointMotor newMotor2 = new JointMotor();
        newMotor2.targetVelocity = motorTargetVelocity;
        newMotor2.force = motorForce;

        var hinge1 = transform.Find("Motion Wheels").GetComponents<HingeJoint>()[0];
        var hinge2 = transform.Find("Motion Wheels").GetComponents<HingeJoint>()[1];

        hinge1.motor = newMotor;
        hinge2.motor = newMotor2;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
