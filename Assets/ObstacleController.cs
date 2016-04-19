using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private List<KeyValuePair<GameObject, float>> hitCoolDowns;

    // Use this for initialization
    void Start()
    {
        hitCoolDowns = new List<KeyValuePair<GameObject, float>>();
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
        for(int i = 0; i < hitCoolDowns.Count; i++)
        {
            if (hitCoolDowns[i].Value + 1.0f < Time.time)
                hitCoolDowns.RemoveAt(i--);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        var colCC = col.gameObject.transform.root.GetComponentInChildren<CarController>();
        if(colCC != null)
        {
            foreach (var existingCD in hitCoolDowns)
                if (existingCD.Key == colCC.gameObject)
                    return;
            Debug.Log("Col btwn " + colCC.gameObject.name + " & " + gameObject.name);
            hitCoolDowns.Add(new KeyValuePair<GameObject, float>(colCC.gameObject, Time.time));

            if(colCC.gameObject.name.Contains("Water"))
            {
                //Here code for effect of water
            }
            else
            {
                colCC.ReduceSpeed();
            }
        }
    }
}
