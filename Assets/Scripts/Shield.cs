using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    bool notCreated = false;
    public void createShield(bool collided, CarController car)
    {
        if (!notCreated)
        {
            if (collided)
            {
                for (int i = 0; i < 1; i++)
                {
                    GameObject shield = Instantiate(Resources.Load("Prefabs/Pickups/Shield")) as GameObject;
                    shield.transform.parent = car.transform;
                    shield.transform.position = car.transform.position;
                    notCreated = true;
                }
            }
        }
    }
}