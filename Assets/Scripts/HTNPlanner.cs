using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;




public class CarState
{
    public CarState(int id, Vector3 pos, Vector3 vel, Vector3 fwd) { myUniqueID = id; myPosition = pos; myVelocity = vel; forward = fwd; }
    public CarState() { }
    public Vector3 myPosition;
    public Vector3 myVelocity;
    public Vector3 forward;
    public int myUniqueID;
}

public class ObstacleState
{
    public ObstacleState(Vector3 pos, Vector3 vel, Vector3 fwd, Bounds bnds) { myBounds = bnds; myPosition = pos; myVelocity = vel; forward = fwd; }
    public ObstacleState() { }
    public Vector3 myPosition;
    public Vector3 myVelocity;
    public Vector3 forward;
    public Bounds myBounds;
    public int myUniqueID;
}
public class State
{
    public State() { }
    public CarState myCar;
    public CarState[] otherCars;
    public ObstacleState[] obstacles;
    public Vector3[] targetPositions;
}

public class HTNPlanner {

    private const float minSpeedToForceAccel = 10.0f; // Speed of car below which you are always accelerating
    private const float minAngleSharpTurn = 60.0f; // Angle in degrees at which you are braking
    private const float minAngleNormalTurn = 30.0f; //Angle in degrees at which you are not accelerating
    private const float rotationAmount = 1.0f; // amount of degrees added per frame on rotations
    private const int timeStepPerDecision = 10; //Number of time steps processed between each state check
    private const int refreshOccurence = 3; // Number of times the data doesn't refresh before it does
    
    public CarState targetCar;
    public Vector3 myTarget;
    /// <summary>
    /// A list containing the names of all tasks of the given planning domain for which there are primitive actions
    /// </summary>
    private List<string> operators;

    /// <summary>
    /// A dictionary containing the names of all non-primitive tasks of the given planning domain as dictionary keys, 
    /// and a list containing the names of all HTN-methods, which can decompose the non-primitive tasks into subtasks, as dictionary values
    /// </summary>
    private Dictionary<string, List<string>> methods = new Dictionary<string, List<string>>();

    private float planningTime;
    private int numberTimeSteps;
    private int currentIteration;
    private float lastTurn;

    private State currentState;
    private Utility myUtility;
    char[][] plan;

    
    public HTNPlanner(float planningTime)
    {
        this.planningTime = planningTime;
        currentIteration = 0;
        numberTimeSteps = Mathf.RoundToInt(planningTime * (1.0f / Time.fixedDeltaTime));
        Dictionary<string, MethodInfo[]> myDict = new Dictionary<string, MethodInfo[]>();
        MethodInfo[] moveInfos = new MethodInfo[] { this.GetType().GetMethod("MoveTo_m") };
        myDict.Add("MoveTo", moveInfos);
        MethodInfo[] moveStepInfos = new MethodInfo[] { this.GetType().GetMethod("MoveStep_m") };
        myDict.Add("MoveStep", moveStepInfos);
        myUtility = new Utility();

        DeclareOperators();
        foreach (KeyValuePair<string, MethodInfo[]> method in myDict)
        {
            DeclareMethods(method.Key, method.Value);
        }
    }


    //Function called by AIController to Get an update version of the Input Keys pressed by the AI
    public string[] GetPlan(State newState, bool isAggresive)
    {
        lastTurn = 0.0f;
        currentState = newState;
        myUtility.CarUtility(newState, isAggresive);

        List<List<string>> tasks = new List<List<string>>();
        tasks.Add(new List<string>(new string[2] { "MoveTo", myTarget.ToString() }));

        
        plan = new char[numberTimeSteps][];
        for(int i = 0; i < plan.Length; i++)
        {
            plan[i] = new char[2];
        }
        SeekPlan(currentState, tasks);
        string[] formattedPlan = new string[numberTimeSteps];
        int u = 0;
        foreach (char[] timestep in plan)
            formattedPlan[u++] = timestep[0].ToString() + timestep[1].ToString();
        return formattedPlan;
    }

    //Recursive function generating the plan
    public char[][] SeekPlan(State state, List<List<string>> tasks)
    {
        if (tasks.Count == 0)
            return plan;
        List<string> task = tasks[0];

        if (operators.Contains(task[0]))
        {
            MethodInfo info = typeof(HTNPlanner).GetMethod(task[0]);
            object[] parameters = new object[task.Count];
            parameters[0] = state;
            if (task.Count > 1)
            {
                int x = 1;
                List<string> paramets = task.GetRange(1, (task.Count - 1));
                parameters[x++] = float.Parse(paramets[0]);
                parameters[x++] = int.Parse(paramets[1]);

            }
            State newState = (State)info.Invoke(this, parameters);
            SeekPlan(newState, tasks.GetRange(1, (tasks.Count - 1)));
        }
        else if (methods.ContainsKey(task[0]))
        {
            List<string> relevant = methods[task[0]];
            foreach (string method in relevant)
            {
                // Decompose non-primitive task into subtasks by use of a HTN method (invoke C# method)
                MethodInfo info = typeof(HTNPlanner).GetMethod(method);
                object[] parameters = new object[task.Count];
                parameters[0] = state;
                if (task.Count > 1)
                {
                    int x = 1;
                    List<string> paramets = task.GetRange(1, (task.Count - 1));
                    foreach (string param in paramets)
                    {
                        parameters[x] = param;
                        x++;
                    }
                }
                List<List<string>> subtasks = null;
                try
                {
                    subtasks = (List<List<string>>)info.Invoke(this, parameters);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }


                if (subtasks != null)
                {
                    List<List<string>> newTasks = new List<List<string>>(subtasks);
                    newTasks.AddRange(tasks.GetRange(1, (tasks.Count - 1)));
                    try
                    {
                        char[][] solution = SeekPlan(state, newTasks);
                        if (solution != null)
                            return solution;
                    }
                    catch (StackOverflowException e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
        return plan;
    }

    public List<string> DeclareOperators()
    {
        MethodInfo[] methodInfos = typeof(HTNPlanner).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        operators = new List<string>();

        foreach (MethodInfo info in methodInfos)
        {
            if (info.ReturnType.Name.Equals("State"))
            {
                string methodName = info.Name;
                if (!operators.Contains(methodName))
                    operators.Add(methodName);
            }
        }

        return operators;
    }

    public List<string> DeclareMethods(string taskName, MethodInfo[] methodInfos)
    {
        List<string> methodList = new List<string>();

        foreach (MethodInfo info in methodInfos)
        {
            if (info != null && info.ReturnType.Name.Equals("List`1"))
            {
                methodList.Add(info.Name);
            }
        }

        if (methods.ContainsKey(taskName))
            methods.Remove(taskName);
        methods.Add(taskName, methodList);

        return methods[taskName];
    }

    /// <summary>
    /// HTN method for finding a way to a room
    /// </summary>
    /// <param name="state">The current internal state</param>
    /// <param name="room">The room to which the robot needs to travel to</param>
    /// <returns>A list of subtasks to get from the current room to the room given in the parameters if a path can be found, else it returns null</returns>
    public List<List<string>> MoveTo_m(State state, string strTargetPosition)
    {
        List<List<string>> returnVal = new List<List<string>>();

        for(int i = 0; i < timeStepPerDecision; i++)
            returnVal.Add(new List<string> { "MoveStep", strTargetPosition, i.ToString() });


        return returnVal;
    }

    public List<List<string>> MoveStep_m(State state, string strTargetPosition, string currentStep)
    {
        Vector3 myDisplacement = myUtility.getDirection(state);
        List<List<string>> returnVal = new List<List<string>>();
        

        myDisplacement.y = 0;
        Vector3 v1 = -state.myCar.forward.normalized;
        Vector3 v2 = myDisplacement.normalized;
        float dotP = Vector3.Dot(v1, v2);
        float angle = Mathf.Acos(dotP) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(v1, v2);
        if (Vector3.Dot(Vector3.up, cross) < 0)
        {
            angle = -angle;
        }


        returnVal.Add(new List<string> { "Rotate", Mathf.RoundToInt(angle).ToString(), currentStep });
        returnVal.Add(new List<string> { "GoForward", myDisplacement.magnitude.ToString(), currentStep });

        return returnVal;
    }

    public State GoForward(State state, float distance, int currentStep)
    { 
        State newState = currentState;
        //Do stuff
        for(int i = currentStep * timeStepPerDecision; i < Mathf.Min(numberTimeSteps, (currentStep + 1)* timeStepPerDecision); i++)
        {
            if (state.myCar.myVelocity.magnitude < minSpeedToForceAccel) //If going very slowly, then always accelrate
                plan[i][0] = 'W';
            else if(lastTurn > minAngleSharpTurn || false) // If currently in a very sharp turn  - slow
                plan[i][0] = 'S';
            else if(lastTurn > minAngleNormalTurn) //If currently in a fairly sharp turn, don't accelerate
                plan[i][0] = 'X';
            else // Else accelerate
                plan[i][0] = 'W';

            if (lastTurn > 0) //Reduce lastTurn by one on each frame to simulate the rotation of the car
                lastTurn--;

        }
        //Do stuff

        return newState;
    }

    public State Rotate(State state, float angle, int currentStep)
    {
        State newState = currentState;
        lastTurn = Mathf.Abs(angle);
        //Do stuff

        int numberRotation = Mathf.Abs(Mathf.CeilToInt(angle / rotationAmount));
        for (int i = currentStep * timeStepPerDecision; i < Mathf.Min(numberTimeSteps, (currentStep + 1) * timeStepPerDecision); i++)
        {
            if (angle > 0 && numberRotation-- > 0)
                plan[i][1] = 'D';
            else if (angle < 0 && numberRotation-- > 0)
                plan[i][1] = 'A';
            else
                plan[i][1] = 'X';
        }
        //Do stuff

        return newState;
    }
}
