using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarHitbox : MonoBehaviour {


    public Slider healthSlider;
    [SerializeField]
    private System.Collections.Generic.List<Transform> DetachableParts;
    //Use this as the list of detachables that are linked with this hitbox

    private GameObject car;
    public Rigidbody carModel;
    private float damageCaused;
    public float carSpeed;

    // Use this for initialization
    void Start () {
        car = GameObject.Find("Player");
    }

    // COLLISION DETECTION
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Obstacle2")
        {
            // We need to find out which sphere collider we are hitting here
            // We choose one random detachable part from the list

            // We check if the list of detachable part has something
            if (DetachableParts.Count > 0) {

            int elementPosition = Random.Range(0, DetachableParts.Count);
            Transform partAffected = DetachableParts[elementPosition];
            // We check if the piece chosen has any health left or has been 
            carSpeed = carModel.velocity.magnitude;
            if (partAffected != null && partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth > 0) {
                // We have health so we make all the calculation with the piece data
                // We calculate the force of the impact with the obstacle
                // Force = (mass * speed * speed)/2
                damageCaused = (partAffected.GetComponent<DetachableElementBehaviour>().pieceMass * carSpeed * carSpeed) / 2;
                // We reduce the health of the detachable part and check if the drop flag must be true
                partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth -= damageCaused;
                // If the piece hasn´t left health we detach the piece
                if (partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth < 0)
                {
                    partAffected.GetComponent<DetachableElementBehaviour>().isHanging = true;
                    // We remove the part from the list so it´s not available any more
                    //DetachableParts[elementPosition] = null;
                    DetachableParts.RemoveAt(elementPosition);
                }
                
            } else {
                // The piece has been detached so we only reduce the health of the car
                damageCaused = (1 * carSpeed * carSpeed) / 2;
            }
            } else  {
                // The piece has been detached so we only reduce the health of the car
                damageCaused = (1 * carSpeed * carSpeed) / 2;
            }
            // We reduce the car health & the slider at the same time (round down to have more health) ***** POSSIBLE CHANGE OF CAR HEALTH TO FLOAT *******
            carModel.GetComponent<CarController>().currentHealth -= Mathf.FloorToInt(damageCaused * 0.1f);
            // The total slider is reduced in a smallest amount
            healthSlider.value = carModel.GetComponent<CarController>().currentHealth;


            // check the health of the car
            // Explosion anitmation ARTISTS
           if (carModel.GetComponent<CarController>().currentHealth < 0)
            {
                // Run explosion

                // Call the co routine to spawn the car after some delay
                StartCoroutine(respawnCar());
                
            }

        }
    }

    IEnumerator respawnCar()
    {
        // Disable the controls in here to stop moving the car when respawn ******* TO DO ********

        yield return new WaitForSeconds(1);

        // Destroy the car
        Destroy(carModel);

        // Logic about respawning later


    }


    // Update is called once per frame
    void Update () {

        // We calculate the speed of the car for the collision force
        carSpeed = carModel.velocity.magnitude;
    }
}
