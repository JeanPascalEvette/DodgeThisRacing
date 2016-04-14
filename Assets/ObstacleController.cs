using UnityEngine;
using System.Collections;

[System.Serializable]
public class MultiDimensional2DVector3
{
    public MultiDimensionalVector3[] ConfigPresetNumber;
}

[System.Serializable]
public class MultiDimensionalVector3
{
    public Vector3[] ObstacleInstance;
}

public class ObstacleController : MonoBehaviour
{


    public MultiDimensional2DVector3[] trackPieceNumber;


    // Use this for initialization
    void Start()
    {
    }
    

    public int GetNumberOfPresets(int trackPartId)
    {
        if (trackPieceNumber == null || trackPieceNumber.Length != Data.GetNumberTrackPartAvailable())
            return 0;
        return trackPieceNumber[trackPartId].ConfigPresetNumber.Length;
    }

    public int GetNumberOfInstances(int trackPartId, int preset)
    {
        if (trackPieceNumber[trackPartId].ConfigPresetNumber[preset] == null || trackPieceNumber[trackPartId].ConfigPresetNumber[preset].ObstacleInstance == null)
            return 0;
        return trackPieceNumber[trackPartId].ConfigPresetNumber[preset].ObstacleInstance.Length;
    }

    public void SetupPosition(int trackPartId, int presetId, int instanceNumber)
    {
        //Chose Random Position
        if(trackPartId < trackPieceNumber.Length && presetId < trackPieceNumber[trackPartId].ConfigPresetNumber.Length && instanceNumber < trackPieceNumber[trackPartId].ConfigPresetNumber[presetId].ObstacleInstance.Length)
            transform.position = transform.parent.position + trackPieceNumber[trackPartId].ConfigPresetNumber[presetId].ObstacleInstance[instanceNumber];
        else
            transform.position = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
