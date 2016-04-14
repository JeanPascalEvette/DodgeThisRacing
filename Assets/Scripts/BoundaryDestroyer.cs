using UnityEngine;
using System.Collections;

public class BoundaryDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("coliision has occured");

        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
