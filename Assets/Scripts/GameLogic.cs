﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

    public int[] scoreCount = {10, 10, 10, 10};
    public int winCondition;
    public bool victory;
    public int playerDeathNumber;

    public readonly int NUMBEROFCARS = 4;


    private List<GameObject> trackPartsList;

    [SerializeField]
    private int NumberOfTrackParts;
    [SerializeField]
    private int NumberOfCars;

    [SerializeField]
    private GameObject Track;

    public List<GameObject> obstacleList;

    public static GameLogic myInstance;

    private Camera myCamera;

    // Use this for initialization
    void Start ()
    {   
        winCondition = 0;
        victory = false;
        playerDeathNumber = 1;

        if (myInstance != null)
            Destroy(gameObject);
        else
            myInstance = this;


        myCamera = UnityEngine.Camera.main.GetComponent<Camera>();

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
        if (data.GetGameObject() != null) {
            return;
        }

        if (scoreCount[data.GetCarType()] != 0) { 
        var spawnPos = new Vector3(0,0.5f,myCamera.leadingGameObject.transform.position.z - 1.0f);
        var spawnVel = myCamera.leadingGameObject.GetComponent<Rigidbody>().velocity;
        var newCar = (GameObject)Instantiate(data.GetPrefab(), spawnPos, Quaternion.Euler(0, 180, 0));
        newCar.GetComponent<Rigidbody>().velocity = spawnVel;
        newCar.GetComponent<CarController>().myPlayerData = data;
            data.AttachGameObject(newCar);
        }
        else
        {
            winCondition++;
            if (winCondition == Data.getNumberCarSelected() - 1)
            {
                for (int n = 0; n < scoreCount.Length; n++)
                {
                    if (scoreCount[n] != 0)
                    {
                        playerDeathNumber = n + 1;
                    }
                }
                victory = true;
            }
        }


    }
	
    public void DestroyCar(PlayerData data)
    {
        if (scoreCount[data.GetCarType()] != 0)
        {
            playerDeathNumber = data.GetCarType() + 1;
            scoreCount[data.GetCarType()]--;


            if (data.IsAI())
                data.GetGameObject().GetComponent<AIController>().stopPlanner();
            Destroy(data.GetGameObject());
        }

    }

    void AddObstacles(GameObject trackPart, int trackPartId)
    {
        int numPresets = Data.getObstacle(0).GetComponent<ObstacleController>().GetNumberOfPresets(trackPartId);
        if (numPresets == 0) return;
        int preset = Random.Range(0, numPresets);
        for (int obstacleNum = 0; obstacleNum < Data.GetNumObstacleAvailable(); obstacleNum++)
        {
            if (numPresets > Data.getObstacle(obstacleNum).GetComponent<ObstacleController>().GetNumberOfPresets(trackPartId))
                continue;
            GameObject obstaclePrefab = Data.getObstacle(obstacleNum);
            for (int i = 0; i < obstaclePrefab.GetComponent<ObstacleController>().GetNumberOfInstances(trackPartId, preset); i++)
            {
                var newObstacle = (GameObject)Instantiate(obstaclePrefab, Vector3.zero, obstaclePrefab.transform.rotation);
                newObstacle.transform.parent = trackPart.transform;
                newObstacle.GetComponent<ObstacleController>().SetupPosition(trackPartId, preset, i);
                obstacleList.Add(newObstacle);
            }
        }
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
        if (trackPartsList.Count != 1)
        {
            AddObstacles(newTrackPart, choice);
        }

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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AIController.showDebug = !AIController.showDebug;
        }

        if (trackPartsList.Count > 2 && trackPartsList[2].transform.position.z <  myCamera.leadingGameObject.transform.position.z)
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
