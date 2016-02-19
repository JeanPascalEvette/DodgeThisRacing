using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject target;
    public float xDist = 10.0f;
    public float yDist = 3.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        var targetPos = target.transform.position;
        var dir = (target.transform.forward) - targetPos;
        dir.Normalize();
        dir *= xDist;
        dir.y = yDist;
        this.transform.position = targetPos + (dir);
        transform.LookAt(target.transform);
    }
}
