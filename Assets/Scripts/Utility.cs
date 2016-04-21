using UnityEngine;
using System.Collections;

public class Utility {

    int ID;
    Vector3 currentVelocity;
    Vector3 currentPosition;
    Vector3 targetPosition;
    Vector3 carDirection;
    float carSpeed;

    // variables for other cars
    CarState[] cars;
    Vector3[] positions;

    public void CarUtility(State state, bool isAggresive)
    {
        //Gets car state
        CarState currentCarState = state.myCar;
        ID = currentCarState.myUniqueID;
        currentPosition = currentCarState.myPosition;
        currentVelocity = currentCarState.myVelocity;
        carSpeed = currentVelocity.magnitude;

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
                    targetPosition = cars[i].myPosition;
                }
            }
        }

        //Or set the target of an AI path if not agressive
        else
        {
            int numberOfPaths = state.targetPositions.Length;
            int selectedPath = 0;
            if(numberOfPaths > 1)
            {
                selectedPath = new System.Random().Next(numberOfPaths);
            }
            targetPosition = state.targetPositions[selectedPath];
        }
    }

    //Return a normalized direction
    public Vector3 getDirection(State state)
    {
        CarState thisCar = state.myCar;

        //Checks number of cars on track
        int numberOfCarsInPlay = state.otherCars.Length;

        //If no other cars then change target position
        if (numberOfCarsInPlay == 0)
        {
            int numberOfPaths = state.targetPositions.Length;
            int selectedPath = 0;
            if (numberOfPaths > 1)
            {
                selectedPath = new System.Random().Next(numberOfPaths);
            }
            targetPosition = state.targetPositions[selectedPath];
        }

        //Get direction for the car
        currentPosition = thisCar.myPosition;
        carDirection = (targetPosition - currentPosition);
        
        //Check for obstacles
        for (int i =0; i < state.obstacles.Length; i++)
        {
            float distance = Vector3.Distance(state.obstacles[i].myPosition, currentPosition);
            //Debug.Log(distance);
            if(distance < 300)
            {
                carDirection = Vector3.Scale(carDirection,new Vector3(-1, 0, 1));
                carSpeed -= 10;
            }
        }
        //Need check for distance between other cars
        for (int i = 0; i < state.otherCars.Length; i++)
        {
            //Vector3 intersection between direction and other car position
            float distance = Vector3.Distance(state.otherCars[i].myPosition, currentPosition);
            if(distance < 300)
            {
                carDirection = Vector3.Scale(carDirection, new Vector3(-1, 0, 1));
                carSpeed -= 10;
            }
        }

        
        carDirection.Normalize();
        return carDirection;
    }
}
