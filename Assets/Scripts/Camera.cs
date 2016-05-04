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
    Vector3 leadingPosition;
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

    public Vector3 GetLeadingPlayerPosition()
    {
        if (leadingGameObject != null)
            return leadingGameObject.transform.position;
        target = Data.GetAllCars();
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == null) continue;
            Vector3 position = target[i].transform.position;
            if (leadingPosition == Vector3.zero || position.z > leadingPosition.z)
            {
                leadingPosition = position;
                leadingGameObject = target[i];
            }
        }
        
            return leadingPosition;
    }

    public float getZoomZDiff()
    {
        float zoom = ((currentZoomOut - minZoomOut) / (maxZoomOut - minZoomOut));
        return zoom * xDist * 3;
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
        target = Data.GetAllCars();
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == null) continue;
            Vector3 position = target[i].transform.position;
            if (position.z > leadingPosition.z)
            {
                leadingPosition = position;
                leadingGameObject = target[i];
            }
        }


        this.transform.position = new Vector3(0, yDist * currentZoomOut, leadingPosition.z - ((1.0f / currentZoomOut) * xDist));
        transform.LookAt(new Vector3(0, 0, leadingPosition.z));
    }

    public void ZoomOut(float newMaxZoomOut = -1)
    {
        if (!zoomOut)
        {
            if (newMaxZoomOut != -1)
                maxZoomOut = newMaxZoomOut;
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
