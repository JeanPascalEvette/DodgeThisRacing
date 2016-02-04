using UnityEngine;
using System.Collections;

public class TurnWheels : MonoBehaviour {

    public float turnX = 0.0f;
    public float turnY = 20.0f;
    public float turnZ = 0.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float carRotation = this.GetComponentInParent<CarProperties>().carRotation;
        float carSpeed = this.GetComponentInParent<CarProperties>().carSpeed;
         
        Transform wheel1 = transform.GetChild(0).transform;
        Transform wheel2 = transform.GetChild(1).transform;
        wheel1.eulerAngles = new Vector3(turnX, turnY, turnZ);
        wheel2.eulerAngles = new Vector3(turnX, turnY, turnZ);
        //wheel1.Rotate(0, carRotation * 2 * Mathf.Deg2Rad, );
        //wheel2.Rotate(0, carRotation * 2 * Mathf.Deg2Rad, -carSpeed * 6 * Time.deltaTime);
    }
}
