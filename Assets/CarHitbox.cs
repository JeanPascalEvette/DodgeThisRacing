using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarHitbox : MonoBehaviour
{
    [SerializeField]
    private Transform[] DetachableParts;
    //Use this as the list of detachables that are linked with this hitbox

    private CarController car;
    public Rigidbody carModel;
    private float damageCaused;
    public float carSpeed;

    private float collisionCD = -1;
    private GameObject explHolder;
    // Use this for initialization
    void Start()
    {
        car = transform.root.GetComponent<CarController>();
        carModel = car.GetComponent<Rigidbody>();

        explHolder = GameObject.Find("ExplosionHolder");
        if (explHolder == null)
            explHolder = new GameObject("ExplosionHolder");
    }

    // COLLISION DETECTION
    void OnTriggerEnter(Collider col)
    {
        if (collisionCD > 0)
            return;
        Debug.Log("COLLISION WITH WALL");

        if (col.transform.root.name == "Main Camera")
            return; //Ignore collision with camera

        if (col.gameObject.name == "Track")
            return; // Ignore collisions with ground
        if (col.transform.root == transform.root)
            return; // Ignore self collisions
        //{
        // We need to find out which sphere collider we are hitting here
        

        Debug.Log("IN COLLISION");

        // We choose one random detachable part from the list
        Debug.Log("PARTS: " + DetachableParts.Length);
        int elementPosition = Random.Range(0, DetachableParts.Length);
        Transform partAffected = DetachableParts[elementPosition];
        // We check if the piece chosen has any health left or has been 
        carSpeed = carModel.velocity.magnitude;
        Debug.Log("CAR SPEED: " + carSpeed);
        if (partAffected != null && partAffected.GetComponent<DetachableElementBehaviour>() != null && partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth > 0)
        {
            // We have health so we make all the calculation with the piece data
            // We calculate the force of the impact with the obstacle
            // Force = (mass * speed * speed)/2
            damageCaused = (partAffected.GetComponent<DetachableElementBehaviour>().pieceMass * carSpeed * carSpeed) / 2;
            Debug.Log("DAMAGE PIECE: " + damageCaused);
            // We reduce the health of the detachable part and check if the drop flag must be true
            partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth -= damageCaused;
            Debug.Log("PART CHOSEN: " + partAffected.name + " -> HEALTH WITH DAMAGE: " + partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth);
            // If the piece hasn´t left health we detach the piece
            if (partAffected.GetComponent<DetachableElementBehaviour>().pieceHealth < 0)
            {
                Debug.Log("Detach part");
                partAffected.GetComponent<DetachableElementBehaviour>().isHanging = true;
                // We remove the part from the list so it´s not available any more
                DetachableParts[elementPosition] = null;
            }

        }
        else
        {
            // The piece has been detached so we only reduce the health of the car
            damageCaused = (1 * carSpeed * carSpeed) / 2;
            Debug.Log("DAMAGE CAR: " + damageCaused);
        }
        // We reduce the car health & the slider at the same time (round down to have more health) ***** POSSIBLE CHANGE OF CAR HEALTH TO FLOAT *******
        Debug.Log("CAR HEALTH BEFORE: " + car.currentHealth);
        car.currentHealth -= Mathf.FloorToInt(damageCaused * 0.01f);
        Debug.Log("CAR HEALTH: " + car.currentHealth);


        RaycastHit hit;
        if (Physics.Raycast(transform.position, (col.transform.position - transform.position), out hit))
        {
            var sparks = (GameObject)Instantiate(Resources.Load("Prefabs/FX/Explosions/CollisionSparks"), hit.point, Quaternion.identity);
            sparks.transform.parent = explHolder.transform;
            sparks.GetComponent<Rigidbody>().velocity = transform.root.GetComponent<Rigidbody>().velocity;
        }


        collisionCD = 0.1f;
        // check the health of the car
        // Explosion anitmation ARTISTS
        //}
    }

    // Update is called once per frame
    void Update()
    {

        // We calculate the speed of the car for the collision force
        carSpeed = carModel.velocity.magnitude;
        if (collisionCD > 0)
            collisionCD -= Time.deltaTime;
    }
    
}



