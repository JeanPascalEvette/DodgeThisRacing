using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

    int ID;
    Vector3 velocity;
    Vector3 position;
    int timeSteps;
    Vector3[] stepPositions;

    void NewUtility(CarState state, int steps)
    {
        ID = state.myUniqueID;
        timeSteps = steps;
        velocity = state.myVelocity;
        position = state.myPosition;
    }

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
