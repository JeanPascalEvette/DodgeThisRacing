using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

    int ID;
    Vector3 currentVelocity;
    Vector3 currentPosition;
    Vector3 targetPosition;
    Vector3 carDirection;

    // variables for other cars
    CarState[] cars;
    Vector3[] positions;

    int attackCar;

    void CarUtility(State state)
    {
        //Gets car state
        CarState currentCarState = state.myCar;
        ID = currentCarState.myUniqueID;
        currentPosition = currentCarState.myPosition;
        currentVelocity = currentCarState.myVelocity;

        //Get position of the other cars
        cars = state.otherCars;
        for (int i = 0; i < cars.Length; i++)
        {
            positions[i] = cars[i].myPosition;
        }
        
        //Set target position
        attackCar = Random.Range(0, 2);

        if(attackCar==1)
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

        else
        {
            float updateTarget = currentPosition.z + currentVelocity.z;
            targetPosition = new Vector3(currentPosition.x, currentPosition.y, updateTarget);
        }

        
        //targetPosition = new Vector3(0, 0, 0); //Using vector3 as a test
    }

    //Return a normalized direction
    Vector3 getDirection(State state)
    {
        CarState thisCar = state.myCar;
        currentPosition = thisCar.myPosition;
        carDirection = (targetPosition - currentPosition);
        carDirection.Normalize();
        return carDirection;
    }


    //void randomPathSelection()
    //{
    //    int numberOfPaths = positions.Length;
    //    if(numberOfPaths>1)
    //    {
    //        int pathNum = Random.Range(0, 2);
    //        nextPosition = positions[pathNum];
    //    }
    //}

    //void CalculateSteps()
    //{
    //    for(int i = 0; i< timeSteps; i++)
    //    {
    //        stepPositions[i].z = (currentVelocity.z * i) + currentPosition.z;
    //    }
    //}    
}
