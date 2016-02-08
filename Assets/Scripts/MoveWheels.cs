using UnityEngine;
using System.Collections;

public class MoveWheels : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float carSpeed = this.GetComponentInParent<CarProperties>().carSpeed;
        var go = transform.GetChild(0).GetChild(0);
        transform.GetChild(0).GetChild(0).transform.Rotate(0, 0, - carSpeed * 12 * Time.deltaTime);
        transform.GetChild(1).GetChild(0).transform.Rotate(0, 0, - carSpeed * 12 * Time.deltaTime);
        //transform.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 0);

    }
} 
