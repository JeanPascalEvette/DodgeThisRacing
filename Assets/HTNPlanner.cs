using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;



/*

    JP NOTES
    - Not currently true HTN because :
        - Not trully planning
        - Not truly recursive
    - Option to improve : 
        MoveTo_m generates NumberTimeSteps/10 MoveTo objects
        Those will each calculate the vector to follow based on utility and simulating positions and then generate 10 input keys based on that


    */


public class GameObjectState
{
    public GameObjectState()
    {

    }
    public Vector3 mPosition;
    public Quaternion mRotation;
}

public class HTNPlanner {

    private const float minSpeedToForceAccel = 10.0f; // Speed of car below which you are always accelerating
    private const float minAngleSharpTurn = 60.0f; // Angle in degrees at which you are braking
    private const float minAngleNormalTurn = 30.0f; //Angle in degrees at which you are not accelerating
    private const float rotationAmount = 1.0f; // amount of degrees added per frame on rotations


    private GameObject myCar;
    private GameObject targetCar;
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

    private GameObjectState currentState;
    char[][] plan;

    
    public HTNPlanner(GameObject myCar, float planningTime)
    {
        this.myCar = myCar;
        this.planningTime = planningTime;
        currentIteration = 0;
        numberTimeSteps = Mathf.RoundToInt(planningTime * (1.0f / Time.fixedDeltaTime));
        Dictionary<string, MethodInfo[]> myDict = new Dictionary<string, MethodInfo[]>();
        MethodInfo[] moveInfos = new MethodInfo[] { this.GetType().GetMethod("MoveTo_m") };
        myDict.Add("MoveTo", moveInfos);

        DeclareOperators();
        foreach (KeyValuePair<string, MethodInfo[]> method in myDict)
        {
            DeclareMethods(method.Key, method.Value);
        }
    }


    //Function called by AIController to Get an update version of the Input Keys pressed by the AI
    public string[] GetPlan()
    {
        Vector3 myTarget;
        lastTurn = 0.0f;
        if (currentIteration++ % 5 != 0)
        {
            Debug.Log("Repeat");
            myTarget = targetCar.transform.position;
        }
        else
        {
            //Here query the Utility section for a goal
            //Then convert it into a Vector3 Target
            //For now let's take an artificial target
            targetCar = GameObject.Find("Ramp");
            myTarget = targetCar.transform.position;
        }

        List<List<string>> tasks = new List<List<string>>();
        tasks.Add(new List<string>(new string[2] { "MoveTo", myTarget.ToString() }));

        currentState = new GameObjectState();
        currentState.mPosition = myCar.transform.position;
        currentState.mRotation = myCar.transform.rotation;
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
    public char[][] SeekPlan(GameObjectState state, List<List<string>> tasks)
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
                foreach (string param in paramets)
                {
                    parameters[x] = float.Parse(param);
                    x++;
                }
            }
            GameObjectState newState = (GameObjectState)info.Invoke(this, parameters);
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
            if (info.ReturnType.Name.Equals("GameObjectState"))
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
    public List<List<string>> MoveTo_m(GameObjectState state, string strTargetPosition)
    {
        string[] targetPosSplit = strTargetPosition.Substring(1, strTargetPosition.Length - 2).Split(',');
        Vector3 targetPosition = new Vector3(float.Parse(targetPosSplit[0]), float.Parse(targetPosSplit[1]), float.Parse(targetPosSplit[2]));
        Vector3 myDisplacement = (targetPosition - myCar.transform.position);
        List<List<string>> returnVal = new List<List<string>>();

        myDisplacement.y = 0;
        Vector3 v1 = -myCar.transform.forward.normalized;
        Vector3 v2 = myDisplacement.normalized;
        float dotP = Vector3.Dot(v1, v2);
        float angle = Mathf.Acos(dotP) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(v1, v2);
        if (Vector3.Dot(Vector3.up, cross) < 0)
        { 
            angle = -angle;
        }

        returnVal.Add(new List<string> { "Rotate", Mathf.RoundToInt(angle).ToString() });
        returnVal.Add(new List<string> { "GoForward", myDisplacement.magnitude.ToString() });

        return returnVal;
    }

    public GameObjectState GoForward(GameObjectState state, float distance)
    {
        GameObjectState newState = currentState;
        bool isAtMaxSpeed = myCar.GetComponent<CarController>().IsAtMaxSpeed();
        //Do stuff
        Debug.Log("Going forward by " + distance);
        foreach(char[] timestep in plan)
        {
            if (myCar.GetComponent<Rigidbody>().velocity.magnitude < minSpeedToForceAccel) //If going very slowly, then always accelrate
                timestep[0] = 'W';
            else if(lastTurn > minAngleSharpTurn || false) // If currently in a very sharp turn  - slow
                timestep[0] = 'S';
            else if(lastTurn > minAngleNormalTurn) //If currently in a fairly sharp turn, don't accelerate
                timestep[0] = 'X';
            else // Else accelerate
                timestep[0] = 'W';

            if (lastTurn > 0) //Reduce lastTurn by one on each frame to simulate the rotation of the car
                lastTurn--;

        }
        //Do stuff

        return newState;
    }

    public GameObjectState Rotate(GameObjectState state, float angle)
    {
        GameObjectState newState = currentState;
        lastTurn = Mathf.Abs(angle);
        //Do stuff
        Debug.Log("Rotating by " + angle);

        int numberRotation = Mathf.Abs(Mathf.CeilToInt(angle / rotationAmount));
        foreach (char[] timestep in plan)
        {
            if (angle > 0 && numberRotation-- > 0)
                timestep[1] = 'D';
            else if (angle < 0 && numberRotation-- > 0)
                timestep[1] = 'A';
            else
                timestep[1] = 'X';
        }
        //Do stuff

        return newState;
    }
}
