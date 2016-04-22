using UnityEngine;
using System.Collections;
using System.Linq;

//This class is designed to go through scenes and to hold data
public class Data : MonoBehaviour {

    private static GameObject[] CarsAvailable;
    private static GameObject[] TrackPartsAvailable;
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
        CarsAvailable = Resources.LoadAll("Prefabs/Vehicules", typeof(GameObject))
             .Cast<GameObject>()
             .ToArray();
        TrackPartsAvailable = Resources.LoadAll("Prefabs/TrackParts", typeof(GameObject))
             .Cast<GameObject>()
             .ToArray();
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

    public static GameObject getTrackPart(int choice = -1)
    {
        if (TrackPartsAvailable == null || TrackPartsAvailable.Length < choice)
            return null;
        if(choice == -1)
            choice = Random.Range(0, TrackPartsAvailable.Length);
        return TrackPartsAvailable[choice];
    }

    public static GameObject getObstacle(int choice = -1)
    {
        if (ObstaclesAvailable == null || ObstaclesAvailable.Length == 0)
            return null;
        if(choice == -1 || choice > ObstaclesAvailable.Length)
            choice = Random.Range(0, ObstaclesAvailable.Length);
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
        float zPos = UnityEngine.Camera.main.GetComponent<Camera>().leadingGameObject.transform.position.z;
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
}
