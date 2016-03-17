using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

    int ID;
    Vector3 velocity;
    Vector3 position;
    int timeSteps;
    Vector3[] stepPositions;
    GameObject[] paths;

    void FindPaths()
    {
        //Use ID to find the attached GameObject
        //Use raycasts to find paths for AI
        //Store number of paths available in list
    }

    void UpdateUtility(CarState state, int steps)
    {
        ID = state.myUniqueID;
        timeSteps = steps;
        velocity = state.myVelocity;
        position = state.myPosition;
    }

    //Need to use direction of travel to update the positions
    Vector3 CalculateFinalPos()
    {
        Vector3 finalPos;
        finalPos = (velocity * timeSteps) + position;
        return finalPos;
    }

    Vector3[] CalculateSteps()
    {
        for(int i = 0; i< timeSteps; i++)
        {
            stepPositions[i] = (velocity * i) + position;
        }
        return stepPositions;
    }    
}
