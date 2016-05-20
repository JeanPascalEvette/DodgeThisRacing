using UnityEngine;
using System.Collections;
using System.Linq;
using System;

//This class is designed to go through scenes and to hold data
public class Data : MonoBehaviour {

    private static GameObject[] CarsAvailable;
    private static GameObject[][] TrackPartsAvailable;
    private static GameObject[] ObstaclesAvailable;
    private static PlayerData[] CarsSelected;
    
    private static GameObject myInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (myInstance != null)
            Destroy(gameObject);
        else
            myInstance = gameObject;

    }

    // Use this for initialization
    void Start ()
    {
        var listTrackDirectories = System.IO.Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + @"\Assets\Resources\Prefabs\TrackParts");
        CarsAvailable = Resources.LoadAll("Prefabs/Vehicules", typeof(GameObject)) 
             .Cast<GameObject>()
             .ToArray();
        TrackPartsAvailable = new GameObject[listTrackDirectories.Length][];
        for (int i = 0; i < listTrackDirectories.Length; i++)
        {
            TrackPartsAvailable[i] = Resources.LoadAll("Prefabs/TrackParts/"+listTrackDirectories[i].Substring(listTrackDirectories[i].LastIndexOf("\\")+1), typeof(GameObject))
                 .Cast<GameObject>()
                 .ToArray();
        }
        ObstaclesAvailable = Resources.LoadAll("Prefabs/Obstacles", typeof(GameObject))
             .Cast<GameObject>()
             .ToArray();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static PlayerData GetPlayerDataFromControlScheme(PlayerData.ControlScheme scheme)
    {
        for(int i = 0; i < CarsSelected.Length; i++)
        {
            if (CarsSelected[i].GetControlScheme() == scheme)
                return CarsSelected[i];
        }
        return null;
    }

    public static int GetNumberTrackPartAvailable()
    {
        return TrackPartsAvailable.Length;
    }

    public static GameObject getTrackPart(int prefab = -1, int preset = -1)
    {
        if (TrackPartsAvailable == null || TrackPartsAvailable.Length < prefab || TrackPartsAvailable[prefab].Length < preset)
            return null;
        if (prefab == -1)
            prefab = UnityEngine.Random.Range(0, TrackPartsAvailable.Length);
        if (preset == -1)
            preset = UnityEngine.Random.Range(0, TrackPartsAvailable[prefab].Length);
        return TrackPartsAvailable[prefab][preset];
    }

    public static GameObject getObstacle(int choice = -1)
    {
        if (ObstaclesAvailable == null || ObstaclesAvailable.Length == 0)
            return null;
        if(choice == -1 || choice > ObstaclesAvailable.Length)
            choice = UnityEngine.Random.Range(0, ObstaclesAvailable.Length);
        return ObstaclesAvailable[choice];
    }

    public static int GetNumObstacleAvailable()
    {
        return ObstaclesAvailable.Length;
    }

    public static GameObject[] generateCars()
    {
        GameObject[] myCars = new GameObject[CarsSelected.Length];
        for(int i = 0; i < CarsSelected.Length; i++)
        {
            myCars[i] = CarsAvailable[CarsSelected[i].GetCarType() % CarsAvailable.Length];
        }
        return myCars;
    }

    public static void selectCars(PlayerData[] cars)
    {
        CarsSelected = cars;
    }

    public static PlayerData[] GetPlayerData()
    {
        return CarsSelected;
    }

    public static GameObject[] GetAllCars()
    {
        GameObject[] allCars = new GameObject[CarsSelected.Length];
        for (int i = 0; i < allCars.Length; i++)
            allCars[i] = CarsSelected[i].GetGameObject();
        return allCars;
    }

    public static GameObject GetCurrentTrackPiece()
    {
        float zPos = UnityEngine.Camera.main.GetComponent<Camera>().GetLeadingPlayerPosition().z;
        for (int i = 0; i < GameObject.Find("Track").transform.childCount; i++)
        {
            var tp = GameObject.Find("Track").transform.GetChild(i);
            var bounds = GameLogic.myInstance.getBoundsOfTrackPiece(tp.GetComponentsInChildren<MeshRenderer>());
            if (bounds.max.z > zPos && bounds.min.z < zPos)
                return tp.gameObject;
        }
        return null;
    }

    public static int getNumberCarSelected()
    {
        if (CarsSelected == null)
            return 0;
        return CarsSelected.Length;
    }

    internal static int GetCountPlayersDead()
    {
        var allPD = GetPlayerData();
        int bodyCount = 0;
        for (int i = 0; i < allPD.Length; i++)
        {
            if (allPD[i].getLives() == 0)
                bodyCount++;
        }
        return bodyCount;
    }

    static int GetSumChanceToOccur()
    {
        int sum = 0;
        for(int i = 0; i < TrackPartsAvailable.Length; i++)
        {
            sum += TrackPartsAvailable[i][0].GetComponent<TrackPieceController>().ChanceToOccur;
        }
        return sum;
    }

    internal static int GetRandomTrackNumber()
    {
        int number = 0;
        number = UnityEngine.Random.Range(0, GetSumChanceToOccur());

        int sum = 0;
        for (int i = 0; i < TrackPartsAvailable.Length; i++)
        {
            sum += TrackPartsAvailable[i][0].GetComponent<TrackPieceController>().ChanceToOccur;
            if (number <= sum)
                return i;
        }

        return -1;
    }
}
