using UnityEngine;
using System.Collections;

public class CarHitbox : MonoBehaviour {


    [SerializeField]
    private Transform[] DetachableParts;
    //Use this as the list of detachables that are linked with this hitbox

	// Use this for initialization
	void Start () {
	
	}

    // COLLISION DETECTION
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("COLLISION WITH WALL");
        if (col.gameObject.name == "Obstacle2")
        {
            // We need to find out which sphere collider we are hitting here
            Debug.Log("IN COLLISION");
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
