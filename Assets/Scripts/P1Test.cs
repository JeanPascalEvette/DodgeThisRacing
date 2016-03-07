using UnityEngine;
using System.Collections;

public class P1Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        HandleMovement();

	}

    void HandleMovement ()
    {
        if (Input.GetKey(KeyCode.RightArrow)) { transform.Translate(2, 0, 0); }
        if (Input.GetKey(KeyCode.LeftArrow)) { transform.Translate(-2, 0, 0); }
        if(Input.GetKey(KeyCode.UpArrow)) { transform.Translate(0, 2, 0); }
        if(Input.GetKey(KeyCode.DownArrow)) { transform.Translate(0, -2, 0); }
    }
}
