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

    public static GameObject getTrackPart()
    {
        if (TrackPartsAvailable == null || TrackPartsAvailable.Length == 0)
            return null;
        int choice = Random.Range(0, TrackPartsAvailable.Length);
        return TrackPartsAvailable[choice];
    }

    public static GameObject getObstacle()
    {
        if (ObstaclesAvailable == null || ObstaclesAvailable.Length == 0)
            return null;
        int choice = Random.Range(0, ObstaclesAvailable.Length);
        return ObstaclesAvailable[choice];
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

    public static PlayerData[] getCarsSelected()
    {
        return CarsSelected;
    }

    public static int getNumberCarSelected()
    {
        if (CarsSelected == null)
            return 0;
        return CarsSelected.Length;
    }
}
