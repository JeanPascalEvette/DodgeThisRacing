using UnityEngine;
using System.Collections;

[System.Serializable]
public class MultiDimensionalVector3
{
    public Vector3[] positionOptions;
}

public class ObstacleController : MonoBehaviour
{


    public MultiDimensionalVector3[] trackOptions;


    // Use this for initialization
    void Start()
    {

    }

    public void SetupPosition(int trackPartId)
    {
        //Chose Random Position
        if(trackPartId < trackOptions.Length && trackOptions[trackPartId].positionOptions.Length > 0)
            transform.position = transform.parent.position + trackOptions[trackPartId].positionOptions[Random.Range(0, trackOptions[trackPartId].positionOptions.Length)];
        else
            transform.position = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
