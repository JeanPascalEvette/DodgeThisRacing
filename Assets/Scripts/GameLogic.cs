using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameLogic : MonoBehaviour
{
    
    public int winner;
    public int lastDeath;

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
    private GameObject explHolder;

    // Use this for initialization
    void Start()
    {
        winner = -1;
        lastDeath = 1;

        if (myInstance != null)
            Destroy(gameObject);
        else
            myInstance = this;


        myCamera = UnityEngine.Camera.main.GetComponent<Camera>();

        if (Track == null)
            Track = GameObject.Find("Track");
        if (Track == null)
            Track = new GameObject("Track");

        explHolder = GameObject.Find("ExplosionHolder");
        if (explHolder == null)
            explHolder = new GameObject("ExplosionHolder");

        PlayerData[] myCars = new PlayerData[NUMBEROFCARS];
        if (Data.getNumberCarSelected() == 0) // IF did not go through main menu (DEBUG ONLY)
        {
            int ctrlScheme = 0;
            myCars[0] = new PlayerData(1,0, (PlayerData.ControlScheme)ctrlScheme++, PlayerData.PlayerType.Player);
            for (int i = 1; i < NUMBEROFCARS; i++)
            {
                myCars[i] = new PlayerData(i+1,i, (PlayerData.ControlScheme)ctrlScheme++, PlayerData.PlayerType.AI);
            }
            Data.selectCars(myCars);
        }
        else
            myCars = Data.GetPlayerData();


        GameObject[] carPrefabs = Data.generateCars();

        for (int i = 0; i < carPrefabs.Length; i++)
        {
            var newCar = (GameObject)Instantiate(carPrefabs[i], new Vector3((i - carPrefabs.Length / 2) * 3.0f, 0, 3.0f), Quaternion.Euler(0, 180, 0));
            var carCtrler = newCar.GetComponent<CarController>();
            carCtrler.myPlayerData = myCars[i];
            Data.GetPlayerData()[i].AttachGameObject(newCar);
            Data.GetPlayerData()[i].AttachPrefab(carPrefabs[i]);
        }


        trackPartsList = new List<GameObject>();
        for (int i = 0; i < NumberOfTrackParts; i++)
        {
            AddNewTrackPart();
        }

    }


    public void SpawnCar(PlayerData data)
    {
        if (data.GetGameObject() != null)
        {
            return;
        }

        if (data.getLives() != 0)
        {

            Vector3 startPoint = myCamera.leadingGameObject.transform.position;
            var trackPiece = Data.GetCurrentTrackPiece();

            if (trackPiece.name.Substring(0, 7) == "Track_D") // If TrackPiece where road is separated - wait for 1sec before respawning
            {

                Ray targetRay = new Ray(new Vector3(-40, 0.5f, myCamera.leadingGameObject.transform.position.z), new Vector3(1, 0, 0));
                RaycastHit[] hits = Physics.RaycastAll(targetRay, 80, 1 << LayerMask.NameToLayer("AIGuide"));
                Vector3[] targetPos = new Vector3[hits.Length];
                for (int i = 0; i < hits.Length; i++)
                    targetPos[i] = hits[i].point;

                if (Vector3.Distance(targetPos[0], myCamera.leadingGameObject.transform.position) < Vector3.Distance(targetPos[1], myCamera.leadingGameObject.transform.position))
                    startPoint = targetPos[0];
                else
                    startPoint = targetPos[1];
            }
            float xPos = 0.0f;
            List<float> listOfXPos = new List<float>();
            foreach (var pData in Data.GetPlayerData())
            {
                if (pData.GetGameObject() != null && pData != data)
                {
                    listOfXPos.Add(pData.GetGameObject().transform.position.x);
                }
            }

            float maxDist = 5.0f;
            for (int i = 0; i < trackPiece.transform.childCount; i++)
            {
                if (trackPiece.transform.GetChild(i).name.Substring(0, 4) == "Road")
                {
                    trackPiece = trackPiece.transform.GetChild(i).gameObject;
                    break;
                }
            }
            for (int i = 0; i < trackPiece.GetComponent<MeshRenderer>().bounds.extents.x; i++)
            {
                bool possible = true;
                foreach (float otherCarXPos in listOfXPos)
                {
                    if (i > otherCarXPos - maxDist && i < otherCarXPos + maxDist)
                    {
                        possible = false;
                    }
                }
                if (possible)
                { xPos = i; break; }
                possible = true;
                foreach (float otherCarXPos in listOfXPos)
                {
                    if (-i > otherCarXPos - maxDist && -i < otherCarXPos + maxDist)
                    {
                        possible = false;
                    }
                }
                if (possible)
                { xPos = -i; break; }
            }
            startPoint.x = xPos;
            startPoint.y = 0f;

            var spawnVel = myCamera.leadingGameObject.GetComponent<Rigidbody>().velocity;

            var newCar = (GameObject)Instantiate(data.GetPrefab(), startPoint, Quaternion.Euler(0, 180, 0));
            newCar.GetComponent<Rigidbody>().velocity = spawnVel;
            newCar.GetComponent<CarController>().myPlayerData = data;
            data.AttachGameObject(newCar);
        }
        else
        {
            var allPD = Data.GetPlayerData();
            int bodyCount = 0;
            int winnerNumber = 0;
            for (int i = 0; i < allPD.Length; i++)
            {
                if (allPD[i].getLives() == 0)
                    bodyCount++;
                else
                    winnerNumber = i + 1;
            }
            if(bodyCount == Data.getNumberCarSelected() - 1)
            {
                winner = winnerNumber;
            }
        }


    }

    public void DestroyCar(PlayerData data, bool respawn = false)
    {
        if (data.getLives() != 0)
        {
            data.reduceLives();
            lastDeath = data.getID();
            string explPrefab = "Prefabs/FX/Explosions/Explosion";
            if (data.GetGameObject().name.IndexOf("Van - Classic") != -1) 
                explPrefab = "Prefabs/FX/Explosions/RaceVanExplosion";
            if (data.GetGameObject().name.IndexOf("Van - Turbo") != -1) 
                explPrefab = "Prefabs/FX/Explosions/RaceVan#2Explosion";
            if (data.GetGameObject().name.Length >= 8 && data.GetGameObject().name.IndexOf("Car - Classic") != -1)
                explPrefab = "Prefabs/FX/Explosions/ClasicMGTExplosion"; 
            if (data.GetGameObject().name.Length >= 8 && data.GetGameObject().name.IndexOf("Car - Racing") != -1)
                explPrefab = "Prefabs/FX/Explosions/RaceMGTEExplosion"; 

             var expl = (GameObject)Instantiate(Resources.Load(explPrefab), data.GetGameObject().transform.position, Quaternion.identity);
            expl.transform.parent = explHolder.transform;
            if (data.IsAI())
                data.GetGameObject().GetComponent<AIController>().stopPlanner();
            Destroy(data.GetGameObject());

            if(respawn)
            {
                SpawnCar(data);
            }
        }
    }



    void AddObstaclesToList(GameObject trackPart)
    {
        var arrayOfChildren = trackPart.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Obstacle").ToArray();
        foreach (var child in arrayOfChildren)
            obstacleList.Add(child.gameObject);
    }

    Bounds GetBoundsOfTrackPiece(GameObject trackPiece)
    {
        Bounds bounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
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
            Bounds previousBounds = getBoundsOfTrackPiece(trackPartsList[trackPartsList.Count - 1].GetComponentsInChildren<MeshRenderer>());
            Bounds currentBounds = getBoundsOfTrackPiece(trackPrefab.GetComponentsInChildren<MeshRenderer>());
            startPos.z += previousBounds.max.z;
            startPos.z -= currentBounds.min.z;


            Vector3 startBorder = previousBounds.max;
            Vector3 endBorder = startBorder;
            endBorder.x = previousBounds.min.x;
            Debug.DrawLine(startBorder, endBorder, Color.yellow, 9999.0f, false);


            Vector3 startBorder2 = previousBounds.min;
            Vector3 endBorder2 = startBorder2;
            endBorder2.x = previousBounds.max.x;
            Debug.DrawLine(startBorder2, endBorder2, Color.blue, 9999.0f, false);


            Debug.DrawLine(trackPartsList[trackPartsList.Count - 1].transform.position + new Vector3(0, 1, 0), startPos + new Vector3(0, 1, 0), Color.red, 9999.0f, false);
        }
        var newTrackPart = (GameObject)Instantiate(trackPrefab, startPos, trackPrefab.transform.rotation);
        trackPartsList.Add(newTrackPart);
        newTrackPart.transform.parent = Track.transform;
        if (trackPartsList.Count != 1)
        {
            //Commented out for new implementage of presets
            AddObstaclesToList(newTrackPart);
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

    public Bounds getBoundsOfTrackPiece(MeshRenderer[] mrList)
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AIController.showDebug = !AIController.showDebug;
        }

        if (trackPartsList.Count > 2 && trackPartsList[2].transform.position.z < myCamera.leadingGameObject.transform.position.z)
        {
            var removedPart = trackPartsList[0];
            for (int i = 0; i < removedPart.transform.childCount; i++)
            {
                obstacleList.Remove(removedPart.transform.GetChild(i).gameObject);
            }
            trackPartsList.Remove(removedPart);
            Destroy(removedPart);
            AddNewTrackPart();
        }

        var trackPiece = Data.GetCurrentTrackPiece();
        if (trackPiece != null && trackPiece.name.Substring(0, 7) == "Track_D")
        {
            if (UnityEngine.Camera.main.GetComponent<Camera>().leadingGameObject.transform.position.z >= trackPiece.transform.Find("Canyon_Middle_D").GetComponent<MeshRenderer>().bounds.max.z)
            {
                UnityEngine.Camera.main.GetComponent<Camera>().ZoomIn();
                //trackPiece.transform.Find("Canyon_Middle_D").GetComponent<MeshRenderer>().enabled = true;
            }
            else if (UnityEngine.Camera.main.GetComponent<Camera>().leadingGameObject.transform.position.z >= trackPiece.transform.Find("Road_D").GetComponent<MeshRenderer>().bounds.min.z)
            {
                UnityEngine.Camera.main.GetComponent<Camera>().ZoomOut();
                //trackPiece.transform.Find("Canyon_Middle_D").GetComponent<MeshRenderer>().enabled = false;
            }
        }

        foreach(PlayerData pd in Data.GetPlayerData())
        {
            if(pd.GetGameObject() != null && pd.GetGameObject().transform.position.y < -2f)
            {
                DestroyCar(pd, true);
            }
        }

    }

    public List<GameObject> GetObstacleList()
    {
        return obstacleList;
    }
}
