using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject target;
    public float xDist = 10.0f;
    public float yDist = 3.0f;
    public float zoom = 1.0f;
    // Use this for initialization

        public enum CameraType
    {
        NORMAL,
        SIDE
    }
    public CameraType myCameraType;

    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        var cameraPos = new Vector3(0, 0, 0);
        switch(myCameraType)
        {
            case CameraType.NORMAL:
                cameraPos = new Vector3(0, 0, -1);
                break;

            case CameraType.SIDE:
                cameraPos = new Vector3(-1, 0, 0);
                break;

        }
        var targetPos = target.transform.position;
        var dir = (target.transform.position  +  cameraPos) - targetPos;
        dir.Normalize();
        dir *= xDist;
        dir.y = yDist;

        dir *= zoom;
        this.transform.position = targetPos + (dir);
        transform.LookAt(target.transform);
    }
}
