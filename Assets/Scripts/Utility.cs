using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

    int ID;
    Vector3 currentVelocity;
    Vector3 currentPosition;
    int timeSteps;
    Vector3[] stepPositions;
    GameObject[] paths;
    Vector3[] positions;
    Vector3 nextPosition;

    Vector3[] CarUtility(State state)
    {
        getInfo(state);
        randomPathSelection();
        CalculateSteps();
        return stepPositions;
    }

    void getInfo(State state)
    {
        CarState currentCarState = state.myCar;
        ID = currentCarState.myUniqueID;
        currentPosition = currentCarState.myPosition;
        currentVelocity = currentCarState.myVelocity;
        positions = state.targetPositions;
    }

    void randomPathSelection()
    {
        int numberOfPaths = positions.Length;
        if(numberOfPaths>1)
        {
            int pathNum = Random.Range(0, 2);
            nextPosition = positions[pathNum];
        }
    }

    void CalculateSteps()
    {
        for(int i = 0; i< timeSteps; i++)
        {
            stepPositions[i].z = (currentVelocity.z * i) + currentPosition.z;
        }
    }    
}
