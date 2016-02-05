using UnityEngine;
using System.Collections;

public class TestSetAngle : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float myRotation = transform.parent.parent.GetComponent<CarProperties>().carRotation;
        float mySpeed = transform.parent.parent.GetComponent<CarProperties>().carSpeed;
        if (this.name == "Left" && myRotation < 0 || this.name == "Right" && myRotation > 0)
            mySpeed /= 2;
        transform.localEulerAngles = new Vector3(myRotation, 0, 0);
        return;
    }
}
