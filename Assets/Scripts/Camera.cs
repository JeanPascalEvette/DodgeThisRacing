using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject[] target;
    public GameObject leadingGameObject;
    public float xDist = 10.0f;
    public float yDist = 3.0f;
    Vector3 leadingCar;
    // Use this for initialization

        public enum CameraType
    {
        NORMAL,
        SIDE
    }
    public CameraType myCameraType;

    void Start () {
	}

    void GetLeadingPlayer()
    {
            target = Data.GetAllCars();
        leadingCar = Vector3.zero;
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == null) continue;
            Vector3 position = target[i].transform.position;
            if(leadingCar == Vector3.zero || position.z > leadingCar.z)
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
        this.transform.position = new Vector3(0,yDist,leadingCar.z - xDist);
        transform.LookAt(new Vector3(0, 0, leadingCar.z));
    }
}
