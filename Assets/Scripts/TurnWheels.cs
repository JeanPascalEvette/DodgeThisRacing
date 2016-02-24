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
        Transform join1 = transform.GetChild(0).transform;
        Transform join2 = transform.GetChild(1).transform;
        Transform wheel1 = join1.GetChild(0).transform;
        Transform wheel2 = join2.GetChild(0).transform;
        join1.eulerAngles = new Vector3(turn, 180,180);
        join2.eulerAngles = new Vector3(turn, 180, 180);
        //wheel1.transform.GetChild(0).Rotate(turn * Mathf.Deg2Rad,0 ,0);
        //wheel2.transform.GetChild(0).Rotate(turn * Mathf.Deg2Rad,0 ,0);
    }
}
