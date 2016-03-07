using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

    private GameObject[] carList;

    private List<GameObject> trackPartsList;

    // Use this for initialization
    void Start () {
        if(Data.getNumberCarSelected() == 0)
        {
            Data.selectCars(new int[] { 0, 1 });
        }
        GameObject[] carPrefabs = Data.generateCars();

        carList = new GameObject[carPrefabs.Length];
        for (int i = 0; i < carPrefabs.Length; i++)
        {
            carList[i] = (GameObject)Instantiate(carPrefabs[i], new Vector3(i * 2.0f, 0, 0), Quaternion.Euler(0, 180, 0));
        }


        trackPartsList = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject trackPrefab = Data.getTrackPart();
            Vector3 startPos = new Vector3(0, 0, 0);
            if (trackPartsList.Count > 0)
            {
                startPos.z += trackPartsList[trackPartsList.Count - 1].GetComponentInChildren<MeshRenderer>().bounds.max.z;
                startPos.z -= trackPrefab.GetComponentInChildren<MeshRenderer>().bounds.min.z;
                Debug.DrawLine(trackPartsList[trackPartsList.Count - 1].transform.position+ new Vector3(0,1,0), startPos + new Vector3(0, 1, 0), Color.red, 9999.0f,false);
            }
            trackPartsList.Add((GameObject)Instantiate(trackPrefab, startPos, Quaternion.Euler(0,0,0)));
        }
        
    }
	
    void OnDrawGizmos()
    {
    }

	// Update is called once per frame
	void Update () {
	
	}
}
