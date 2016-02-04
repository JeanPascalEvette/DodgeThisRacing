using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject target;
    public float distance = 20.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y,  target.transform.position.z - distance);
        transform.LookAt(target.transform);
    }
}
