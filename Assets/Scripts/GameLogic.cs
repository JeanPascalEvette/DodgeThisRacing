﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

    private readonly int NUMBEROFCARS = 4;


    private List<GameObject> trackPartsList;

    [SerializeField]
    private int NumberOfTrackParts;
    [SerializeField]
    private int NumberOfCars;

    [SerializeField]
    private GameObject Track;

    public List<GameObject> obstacleList;

    public static GameLogic myInstance;

    // Use this for initialization
    void Start ()
    {
        if (myInstance != null)
            Destroy(gameObject);
        else
            myInstance = this;


        if (Track == null)
            Track = GameObject.Find("Track");
        if (Track == null)
            Track = new GameObject("Track");

        PlayerData[] myCars = new PlayerData[NUMBEROFCARS];
        if (Data.getNumberCarSelected() == 0) // IF did not go through main menu (DEBUG ONLY)
        {
            int ctrlScheme = 0;
            myCars[0] = new PlayerData(0, (PlayerData.ControlScheme)ctrlScheme++, PlayerData.PlayerType.Player);
            for (int i = 1; i < NUMBEROFCARS; i++)
            {
                myCars[i] = new PlayerData(i, (PlayerData.ControlScheme)ctrlScheme++, PlayerData.PlayerType.AI);
            }
            Data.selectCars(myCars);
        }
        else
            myCars = Data.getCarsSelected();


        GameObject[] carPrefabs = Data.generateCars();

        for (int i = 0; i < carPrefabs.Length; i++)
        {
            var newCar = (GameObject)Instantiate(carPrefabs[i], new Vector3((i - carPrefabs.Length/2) * 3.0f, 0, 3.0f), Quaternion.Euler(0, 180, 0));
            var carCtrler = newCar.GetComponent<CarController>();
            carCtrler.myPlayerData = myCars[i];
            Data.getCarsSelected()[i].AttachGameObject(newCar);
            Data.getCarsSelected()[i].AttachPrefab(carPrefabs[i]);
        }


        trackPartsList = new List<GameObject>();
        for (int i = 0; i < NumberOfTrackParts; i++)
        {
            AddNewTrackPart();
        }
        
    }
	
    public void SpawnCar(PlayerData data)
    {
            var newCar = (GameObject)Instantiate(data.GetPrefab(), new Vector3(0,0,0), Quaternion.Euler(0, 180, 0));
            data.AttachGameObject(newCar);
    }
	
    void AddNewObstacle(GameObject trackPart, int trackPartId)
    {
        GameObject obstaclePrefab = Data.getObstacle();
        Vector3 startPos = trackPart.transform.position;
        Bounds trackBounds = trackPart.GetComponentInChildren<MeshRenderer>().bounds;
        var xPos = Random.Range(trackBounds.min.x*0.6f, trackBounds.max.x*0.6f);
        var yPos = startPos.y;
        var zPos = Random.Range(trackBounds.min.z, trackBounds.max.z);
        var newObstacle = (GameObject)Instantiate(obstaclePrefab, Vector3.zero, obstaclePrefab.transform.rotation);
        newObstacle.transform.parent = trackPart.transform;
        newObstacle.GetComponent<ObstacleController>().SetupPosition(trackPartId);
        obstacleList.Add(newObstacle);

    }

    void AddNewTrackPart()
    {
        GameObject trackPrefab;
        int choice = Random.Range(0, Data.GetNumberTrackPartAvailable());
        if (trackPartsList.Count == 0)
            trackPrefab = Data.getTrackPart(0);
        else
            trackPrefab = Data.getTrackPart(choice);
        Vector3 startPos = new Vector3(0, 0, 0);
        if (trackPartsList.Count > 0)
        {
            startPos.z += trackPartsList[trackPartsList.Count - 1].GetComponentInChildren<MeshRenderer>().bounds.max.z;
            startPos.z -= trackPrefab.GetComponentInChildren<MeshRenderer>().bounds.min.z;


            Vector3 startBorder = trackPartsList[trackPartsList.Count - 1].GetComponentInChildren<MeshRenderer>().bounds.max;
            Vector3 endBorder = startBorder;
            endBorder.x = trackPartsList[trackPartsList.Count - 1].GetComponentInChildren<MeshRenderer>().bounds.min.x;
            Debug.DrawLine(startBorder, endBorder, Color.yellow, 9999.0f, false);


            Vector3 startBorder2 = trackPartsList[trackPartsList.Count - 1].GetComponentInChildren<MeshRenderer>().bounds.min;
            Vector3 endBorder2 = startBorder2;
            endBorder2.x = trackPartsList[trackPartsList.Count - 1].GetComponentInChildren<MeshRenderer>().bounds.max.x;
            Debug.DrawLine(startBorder2, endBorder2, Color.blue, 9999.0f, false);


            Debug.DrawLine(trackPartsList[trackPartsList.Count - 1].transform.position + new Vector3(0, 1, 0), startPos + new Vector3(0, 1, 0), Color.red, 9999.0f, false);
        }
        var newTrackPart = (GameObject)Instantiate(trackPrefab, startPos, trackPrefab.transform.rotation);
        trackPartsList.Add(newTrackPart);
        newTrackPart.transform.parent = Track.transform;
        if(trackPartsList.Count != 1)
            AddNewObstacle(newTrackPart, choice);

        if (trackPartsList.Count == NumberOfTrackParts)
        {
            DestroyImmediate(Track.GetComponent<Collider>());


            float minx = float.MaxValue;
            float minz = float.MaxValue;
            float maxx = float.MinValue;
            float maxz = float.MinValue;
            foreach (var trackPart in trackPartsList)
            {
                Bounds trackBounds = getBoundsOfTrackPiece(trackPart.GetComponentsInChildren<MeshRenderer>());
                if (minx > trackBounds.min.x)
                    minx = trackBounds.min.x;
                if (maxx < trackBounds.max.x)
                    maxx = trackBounds.max.x;
                if (minz > trackBounds.min.z)
                    minz = trackBounds.min.z;
                if (maxz < trackBounds.max.z)
                    maxz = trackBounds.max.z;
            }
            BoxCollider newCollider = Track.AddComponent<BoxCollider>();
            Vector3 newCenter = new Vector3((minx + maxx) / 2.0f, 0, (minz + maxz) / 2.0f);
            newCollider.center = newCenter;
            newCollider.size = new Vector3((maxx - minx), 0, (maxz - minz));
        }
    }

    Bounds getBoundsOfTrackPiece(MeshRenderer[] mrList)
    {
        Bounds trackBounds = new Bounds();

        float minx = float.MaxValue;
        float minz = float.MaxValue;
        float maxx = float.MinValue;
        float maxz = float.MinValue;
        foreach (MeshRenderer mr in mrList)
        {
            if (minx > mr.bounds.min.x)
                minx = mr.bounds.min.x;
            if (maxx < mr.bounds.max.x)
                maxx = mr.bounds.max.x;
            if (minz > mr.bounds.min.z)
                minz = mr.bounds.min.z;
            if (maxz < mr.bounds.max.z)
                maxz = mr.bounds.max.z;
        }

        trackBounds.SetMinMax(new Vector3(minx, 0, minz), new Vector3(maxx, 0, maxz));

        return trackBounds;
    }

    void OnDrawGizmos()
    {
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }

	    if(trackPartsList.Count > 2 && trackPartsList[2].transform.position.z < Data.getCarsSelected()[0].GetGameObject().transform.position.z)
        {
            var removedPart = trackPartsList[0];
            for( int i = 0; i < removedPart.transform.childCount; i++)
            {
                obstacleList.Remove(removedPart.transform.GetChild(i).gameObject);
            }
            trackPartsList.Remove(removedPart);
            Destroy(removedPart);
            AddNewTrackPart();
        }
	}

    public List<GameObject> GetObstacleList()
    {
        return obstacleList;
    }
}
