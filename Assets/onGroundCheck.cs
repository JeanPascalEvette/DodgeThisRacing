using UnityEngine;
using System.Collections;

public class onGroundCheck : MonoBehaviour {


	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// sloppy way to check if wheels are on ground
	void OnCollisionStay(Collision other){
		firstMovement.onGround = true;
	}

	void OnCollisionExit(Collision other){

		firstMovement.onGround = false;
	}
}
