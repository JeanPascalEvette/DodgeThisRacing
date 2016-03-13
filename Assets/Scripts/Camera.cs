using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject[] target;
    public GameObject leadingGameObject;
    public float xDist = 10.0f;
    public float yDist = 3.0f;
    Vector3 leadingCar = new Vector3(100,100,100);
    // Use this for initialization

        public enum CameraType
    {
        NORMAL,
        SIDE
    }
    public CameraType myCameraType;

    void Start () {
        target = GameObject.FindGameObjectsWithTag("Player");
	}

    void GetLeadingPlayer()
    {
        for (int i = 0; i < target.Length; i++)
        {
            Vector3 position = target[i].transform.position;
            if(position.z < leadingCar.z)
            {
                leadingCar = position;
                leadingGameObject = target[i];
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetLeadingPlayer();
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
        var targetPos = leadingGameObject.transform.position;
        var dir = (leadingGameObject.transform.position  +  cameraPos) - targetPos;
        dir.Normalize();
        dir *= xDist;
        dir.y = yDist;
        this.transform.position = targetPos + (dir);
        transform.LookAt(leadingGameObject.transform);
    }
}
