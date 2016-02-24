using UnityEngine;
using System.Collections;

public class firstMovement : MonoBehaviour {

	public float forcePower;
	private Rigidbody rb;
	public static bool onGround;	// both wheels must be on ground, because if vehicle is half in air the mass decreases and there is a sudden increase in speed (due to the force we apply)

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(onGround == true){
		rb.AddForce(transform.right * forcePower);
		}

		if(Input.GetKey(KeyCode.Space)){
			forcePower += 1.02f;
		}
		if(Input.GetKeyUp(KeyCode.Space)){
			forcePower = 0;
		}
	}
}
