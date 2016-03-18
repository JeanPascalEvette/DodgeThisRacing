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
    private bool showDebug = false;

    [SerializeField]
    private GUISkin aSkin;

    private CarController myCarController;


    // Use this for initialization
    void Start()
    {
        if (aSkin == null) aSkin = (GUISkin)Resources.Load("AILabel");
        if (aSkin == null) aSkin = new GUISkin();
        myCarController = GetComponent<CarController>();
        allCars = GameObject.FindGameObjectsWithTag("Player");
        planner = new HTNPlanner(1.5f);
        plannerThread = new Thread(retrievePlanner); // TODO IMPLEMENT THREADING
        plannerThread.Start();

    }
    public string GetPlan()
    {
        return plan[frameCounter - frameGenerated];
    }

    void OnDrawGizmos()
    {

        if (!showDebug) return;
        //This draws the list of commands than an AI controlled car receives - might want to show/hide it based on some input at some point
        if (plan != null && plan.Length > frameCounter - frameGenerated && frameCounter - frameGenerated >= 0)
        {
            var style = new GUIStyle(aSkin.GetStyle("box"));
            style.alignment = TextAnchor.MiddleCenter;
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
            if (planner.targetCar != null)
            {
                Vector3 dirGO = getCarByUniqueID(planner.targetCar.myUniqueID).transform.position - transform.position;
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.ArrowCap(1, transform.position, Quaternion.LookRotation(dirGO.normalized), Mathf.Min(10, dirGO.magnitude));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            showDebug = !showDebug;
        }
    }

    void FixedUpdate()
    {
        //AI STUFF
        if (frameCounter++ % (int)(1.0f / Time.fixedDeltaTime) == 0)
        {
            currentState = new State();// Generate a state representing the world to be passed to the HTNPlanner
            currentState.myCar = new CarState(myCarController.carUniqueID, transform.position, GetComponent<Rigidbody>().velocity, transform.forward);
            if (allCars.Length > 0)
            {
                currentState.otherCars = new CarState[allCars.Length - 1];
                int otherCarCount = 0;
                foreach (GameObject car in allCars)
                {
                    if (car == gameObject) continue;
                    currentState.otherCars[otherCarCount++] = new CarState(car.GetComponent<CarController>().carUniqueID, car.transform.position, car.GetComponent<Rigidbody>().velocity, car.transform.forward);
                }
            }
            //Set the waitHandle to make sure that the planner can retrieve a new planning
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



            plan = planner.GetPlan(currentState); //Retrieve updated plan based on currentState
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

}

