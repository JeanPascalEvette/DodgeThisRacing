using UnityEngine;
using System.Collections;

public class testMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        this.GetComponent<Rigidbody>().AddForce(0, 0, 10);
	
	}
}
