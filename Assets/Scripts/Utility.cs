using UnityEngine;
using System.Collections;

public class Utility {

    int ID;
    Vector3 currentVelocity;
    Vector3 currentPosition;
    Vector3 targetPosition;
    Vector3 carDirection;
    float carSpeed;
    bool agression;

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
        carSpeed = currentVelocity.magnitude;
        agression = isAggresive;

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

        //Reset target if already passed
        if(state.myCar.myPosition.z > targetPosition.z)
        {
            CarUtility(state, agression);
        }

        //Get direction for the car
        currentPosition = thisCar.myPosition;
        carDirection = (targetPosition - currentPosition);
        
        //Check for obstacles
        for (int i =0; i < state.obstacles.Length; i++)
        {
            float xDif;
            float zDif;
            float diag;
            xDif = state.obstacles[i].myPosition.x - currentPosition.x;
            zDif = state.obstacles[i].myPosition.z - currentPosition.z;
            diag = Mathf.Sqrt((xDif * xDif) + (zDif * zDif));

            float distance = Vector3.Distance(state.obstacles[i].myPosition, currentPosition);
            if (distance < 30)
            {
                carDirection = Vector3.Scale(carDirection, new Vector3(-1, 0, 1));
                carSpeed -= 10;
            }
        }
        //Need check for distance between other cars
        for (int i = 0; i < state.otherCars.Length; i++)
        {
            float xDif;
            float zDif;
            float diag;
            xDif = state.otherCars[i].myPosition.x - currentPosition.x;
            zDif = state.otherCars[i].myPosition.z - currentPosition.z;
            diag = Mathf.Sqrt((xDif * xDif) + (zDif * zDif));
            //Vector3 intersection between direction and other car position
            float distance = Vector3.Distance(state.otherCars[i].myPosition, currentPosition);
            if(distance < 30)
            {
                carDirection = Vector3.Scale(carDirection, new Vector3(-1, 0, 1));
                carSpeed -= 10;
            }
        }

        
        carDirection.Normalize();
        return carDirection;
    }
}
