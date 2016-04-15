using UnityEngine;
using System.Collections;

public class Utility {

    int ID;
    Vector3 currentVelocity;
    Vector3 currentPosition;
    Vector3 targetPosition;
    Vector3 carDirection;

    // variables for other cars
    CarState[] cars;
    Vector3[] positions;
    

    public Vector3 CarUtility(State state, bool isAggresive)
    {
        //Gets car state
        CarState currentCarState = state.myCar;
        ID = currentCarState.myUniqueID;
        currentPosition = currentCarState.myPosition;
        currentVelocity = currentCarState.myVelocity;

        //Get position of the other cars
        cars = state.otherCars;
        positions = new Vector3[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            positions[i] = cars[i].myPosition;
        }
        
        //Sets the target to car if aggresive
        if(isAggresive)
        {
            float temp = 1000;
            float shortestDistance = 1000;
            for(int i = 0; i < cars.Length; i++)
            {
                temp = Vector3.Distance(currentPosition, positions[i]);
                if(temp < shortestDistance)
                {
                    targetPosition = cars[i].myPosition + cars[i].myVelocity;
                }
            }
        }

        //Or set the target of an AI path if not agressive
        else
        {
            int numberOfPaths = state.targetPositions.Length;
            int selectedPath = 0;
            for(int i = 0; i < state.targetPositions.Length; i++)
            {
                if (Vector3.Distance(state.targetPositions[i], state.myCar.myPosition) < Vector3.Distance(state.targetPositions[selectedPath], state.myCar.myPosition))
                    selectedPath = i;
            }
            targetPosition = state.targetPositions[selectedPath];
        }

        return targetPosition;
    }
    

    //Return a normalized direction
    public Vector3 getDirection(State state)
    {
        CarState thisCar = state.myCar;
        currentPosition = thisCar.myPosition;
        carDirection = (targetPosition - currentPosition);
        carDirection.Normalize();
        return carDirection;
    }
}
