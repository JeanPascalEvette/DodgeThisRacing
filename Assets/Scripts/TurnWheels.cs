using UnityEngine;
using System.Collections;

public class TurnWheels : MonoBehaviour {
    
    public float turn = 20.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float carRotation = this.GetComponentInParent<CarProperties>().carRotation;
        float carSpeed = this.GetComponentInParent<CarProperties>().carSpeed;
         
        Transform wheel1 = transform.GetChild(0).transform;
        Transform wheel2 = transform.GetChild(1).transform;
        //wheel1.eulerAngles = new Vector3(wheel1.eulerAngles.x, wheel1.eulerAngles.y, turnZ);
        //wheel2.eulerAngles = new Vector3(wheel1.eulerAngles.x, wheel1.eulerAngles.y, turnZ);
        wheel1.Rotate(turn * Mathf.Deg2Rad,0 ,0);
        wheel2.Rotate(turn * Mathf.Deg2Rad,0 ,0);
    }
}
