using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        HandleMovement();

    }


    float move_player = 5.0f;

    public bool notSelected = true;
  
    void HandleMovement()
    {
        if (notSelected)
        {
            if (Input.GetKey(KeyCode.RightArrow)) { transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.LeftArrow)) { transform.Translate(-move_player, 0, 0); }
            if (Input.GetKey(KeyCode.UpArrow)) { transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { transform.Translate(0, -move_player, 0); }
        }
        

    }



}


