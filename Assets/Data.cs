using UnityEngine;
using System.Collections;
using System.Linq;

//This class is designed to go through scenes and to hold data
public class Data : MonoBehaviour {

    private static GameObject[] CarsAvailable;
    private static GameObject[] TrackPartsAvailable;
    private static int[] CarsSelected;
    

    void Awake()
    {
        DontDestroyOnLoad(this);
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
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static GameObject getTrackPart()
    {
        if (TrackPartsAvailable == null || TrackPartsAvailable.Length == 0)
            return null;
        return TrackPartsAvailable[0];
    }

    public static GameObject[] generateCars()
    {
        GameObject[] myCars = new GameObject[CarsSelected.Length];
        for(int i = 0; i < CarsSelected.Length; i++)
        {
            myCars[i] = CarsAvailable[CarsSelected[i]];
        }
        return myCars;
    }

    public static void selectCars(int[] cars)
    {
        CarsSelected = cars;
    }

    public static int getNumberCarSelected()
    {
        if (CarsSelected == null)
            return 0;
        return CarsSelected.Length;
    }
}
