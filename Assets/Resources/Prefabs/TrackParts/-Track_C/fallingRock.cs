using UnityEngine;
using System.Collections;

public class fallingRock : MonoBehaviour {

    Transform rock;

    // Use this for initialization
    void Start () {

       rock =  this.transform.GetChild(0);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        rock.gameObject.SetActive(true);
        print("HERE!!!!");
    }
}
