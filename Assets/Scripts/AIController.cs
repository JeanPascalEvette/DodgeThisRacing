using UnityEngine;
using System.Collections;
using System.Threading;

public class AIController : MonoBehaviour
{


    private string[] plan;
    private int frameGenerated;
    private HTNPlanner planner;
    private Thread plannerThread;
    private readonly EventWaitHandle waitHandle = new AutoResetEvent(false);

    private State currentState;
    private int frameCounter = 0;
    private GameObject[] allCars;

    [SerializeField]
    public static bool showDebug = false;

    [SerializeField]
    private GUISkin aSkin;

    private CarController myCarController;
    private float rayWidth = 80.0f;


    private System.Random myRand;
    private bool isAggresive;

    // Use this for initialization
    void Start()
    {
        if (aSkin == null) aSkin = (GUISkin)Resources.Load("AILabel");
        if (aSkin == null) aSkin = new GUISkin();
        myCarController = GetComponent<CarController>();
        allCars = Data.GetAllCars();
        planner = new HTNPlanner(1.5f);
        plannerThread = new Thread(retrievePlanner); // TODO IMPLEMENT THREADING
        plannerThread.Start();
        myRand = new System.Random(System.Guid.NewGuid().GetHashCode());
    }
    public string GetPlan()
    {
        try
        {
            if (plan == null || frameCounter - frameGenerated < 0 || frameCounter - frameGenerated >= plan.Length)
                return "";
            return plan[frameCounter - frameGenerated];
        }
        catch(System.Exception ex)
        {
            Debug.Log("Error when Getting plan. Search value " + (frameCounter - frameGenerated).ToString() + " in array size " + plan.Length);
            return "";
        }
    }

    void OnDrawGizmos()
    {

        if (!showDebug) return;
        //This draws the list of commands than an AI controlled car receives - might want to show/hide it based on some input at some point
        if (plan != null && plan.Length > frameCounter - frameGenerated && frameCounter - frameGenerated >= 0)
        {
            var style = new GUIStyle(aSkin.GetStyle("box"));
            style.alignment = TextAnchor.MiddleCenter;
            if (isAggresive)
                style.normal.textColor = Color.red;
            else
                style.normal.textColor = Color.blue;
            var textPlan = "";
            System.Collections.Generic.List<string> commands = new System.Collections.Generic.List<string>();
            int counter = 1;
            for (int i = 0; i < 1.0f / Time.fixedDeltaTime; i++)
            {
                if (commands.Count > 0 && frameCounter - frameGenerated + i < plan.Length && commands[commands.Count - 1] == plan[frameCounter - frameGenerated + i])
                {
                    counter++;
                }
                else if (plan.Length > frameCounter - frameGenerated + i && frameCounter - frameGenerated + i > 0)
                {
                    if (commands.Count > 0)
                    {
                        commands[commands.Count - 1] += "(x" + counter + ")";
                        counter = 1;
                    }
                    commands.Add(plan[frameCounter - frameGenerated + i]);
                }
            }
            commands[commands.Count - 1] += "(x" + counter + ")";
            for (int i = 0; i < Mathf.Min(5, commands.Count); i++)
            {
                textPlan = textPlan + commands[i] + "\n";
            }
            textPlan = textPlan.Substring(0, textPlan.Length - 1);
            UnityEditor.Handles.Label(transform.position, textPlan, style);


            Vector3 dir = planner.myTarget - transform.position;
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.ArrowCap(0, transform.position, Quaternion.LookRotation(dir.normalized), Mathf.Min(10, dir.magnitude));
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawSolidDisc(planner.myTarget, Vector3.up, 1.0f);

            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < currentState.targetPositions.Length; i++)
            {
                UnityEditor.Handles.DrawSolidDisc(currentState.targetPositions[i], Vector3.up, 0.5f);
            }

            Vector3 midPos = transform.position + new Vector3(0, 0.5f, 25f);
            UnityEditor.Handles.DrawLine(midPos + new Vector3(1, 0, 0) * rayWidth / 2, midPos - new Vector3(1, 0, 0) * rayWidth / 2);
        }


    }

    // Update is called once per frame
    void Update()
    {


    }

    void FixedUpdate()
    {
        //AI STUFF
        if (frameCounter++ % (int)(1.0f / Time.fixedDeltaTime) == 0)
        {
            isAggresive = myRand.Next(0, 11) == 0;
            if (currentState != null && currentState.otherCars != null && currentState.otherCars.Length == 0)
                isAggresive = false;
            currentState = new State();// Generate a state representing the world to be passed to the HTNPlanner
            currentState.myCar = new CarState(myCarController.carUniqueID, transform.position, GetComponent<Rigidbody>().velocity, transform.forward);
            if (allCars.Length > 0)
            {
                int otherCarCount = 0;

                foreach (GameObject car in allCars)
                {
                    if (car == gameObject) continue;
                    if (car == null) continue;
                    otherCarCount++;
                }
                currentState.otherCars = new CarState[otherCarCount];
                otherCarCount = 0;
                foreach (GameObject car in allCars)
                {
                    if (car == gameObject) continue;
                    if (car == null) continue;
                    currentState.otherCars[otherCarCount++] = new CarState(car.GetComponent<CarController>().carUniqueID, car.transform.position, car.GetComponent<Rigidbody>().velocity, car.transform.forward);
                }
            }

            if (GameLogic.myInstance != null)
            {
                System.Collections.Generic.List<GameObject> obstacleList = GameLogic.myInstance.obstacleList;
                ObstacleState[] obstacles = new ObstacleState[obstacleList.Count];

                for (int i = 0; i < obstacleList.Count; i++)
                {
                    Vector3 vel = new Vector3(0, 0, 0);
                    Rigidbody obstacleRB = obstacleList[i].transform.GetComponent<Rigidbody>();
                    if (obstacleRB != null)
                        vel = obstacleRB.velocity;
                    obstacles[i] = new ObstacleState(obstacleList[i].transform.position, vel, obstacleList[i].transform.forward, obstacleList[i].GetComponent<MeshRenderer>().bounds);
                }
                currentState.obstacles = obstacles;
            }
            //Set the waitHandle to make sure that the planner can retrieve a new planning


            float zPos = Mathf.Max(myCarController.GetComponent<Rigidbody>().velocity.z * 1.0f, 5.0f);
            Vector3 midPos = transform.position + new Vector3(0, 0.5f, zPos);

            Ray targetRay = new Ray(midPos + new Vector3(1, 0, 0) * rayWidth/2, - new Vector3(1, 0, 0));
            RaycastHit[] hits = Physics.RaycastAll(targetRay, rayWidth, 1 << LayerMask.NameToLayer("AIGuide"));
            Vector3[] targetPos = new Vector3[hits.Length];
            for (int i = 0; i < hits.Length; i++)
                targetPos[i] = hits[i].point;
            currentState.targetPositions = targetPos;

            if (currentState.targetPositions.Length == 0)
            {
                GameLogic.myInstance.DestroyCar(myCarController.myPlayerData, true);
                Debug.LogWarning("Error : Could not find target position for car " + transform.gameObject.name + ". Killing car");
            }

            waitHandle.Set();

        }
        //END AI STUFF

    }

    GameObject getCarByUniqueID(int id)
    {
        foreach (var car in allCars)
            if (car.GetComponent<CarController>().carUniqueID == id)
                return car;
        return null;
    }

    void retrievePlanner()
    {
        while (true) //Loop continuously after started
        {
            waitHandle.WaitOne(); //Run only if the handle has been set in fixedUpdate (i.e every 1sec)


            plan = planner.GetPlan(currentState, isAggresive); //Retrieve updated plan based on currentState
            //Log generated plan
            frameGenerated = frameCounter;
            string debugPlan = "";
            foreach (string timeStep in plan)
                debugPlan += timeStep + ",";
            debugPlan = debugPlan.Substring(0, debugPlan.Length - 1);
            Debug.Log("Car:" + currentState.myCar.myUniqueID + " - " + debugPlan);

            //Wait for 1sec before calling the planner again
            waitHandle.Reset();
        }
    }


    public void stopPlanner()
    {
        plannerThread.Abort();
    }
}

