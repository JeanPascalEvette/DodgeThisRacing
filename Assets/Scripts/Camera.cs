using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject[] target;
    public GameObject leadingGameObject;
    public float xDist = 10.0f;
    public float yDist = 3.0f;
    private float currentZoomOut = 1.0f;
    private float minZoomOut = 1.0f;
    private float maxZoomOut = 3.0f;
    Vector3 leadingCar;
    bool zoomOut = false;
    float zoomOutTime;
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

    public float getZoomZDiff()
    {
        float zoom = ((currentZoomOut - minZoomOut) / (maxZoomOut - minZoomOut));
        return zoom * xDist;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (zoomOut && currentZoomOut < maxZoomOut)
        {
            currentZoomOut = Mathf.Lerp(minZoomOut, maxZoomOut, Time.time - zoomOutTime);
        }
        else if (!zoomOut && currentZoomOut > minZoomOut)
        {
            currentZoomOut = Mathf.Lerp(maxZoomOut, minZoomOut, Time.time - zoomOutTime);
        }

        GetLeadingPlayer();
        this.transform.position = new Vector3(0, yDist * currentZoomOut, leadingCar.z - ((1.0f / currentZoomOut) * xDist));
        transform.LookAt(new Vector3(0, 0, leadingCar.z));
    }

    public void ZoomOut()
    {
        if (!zoomOut)
        {
            zoomOut = true;
            zoomOutTime = Time.time;
        }
    }

    public void ZoomIn()
    {
        if (zoomOut)
        {
            zoomOut = false;
            zoomOutTime = Time.time;
        }
    }
}
