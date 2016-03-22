using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarHitbox : MonoBehaviour {


    public Slider healthSlider;
    [SerializeField]
    private Transform[] DetachableParts;
    //Use this as the list of detachables that are linked with this hitbox


    public Rigidbody carModel;
    private float damageCaused;
    public float carSpeed;

    // Use this for initialization
    void Start () {
	
	}

    // COLLISION DETECTION
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("COLLISION WITH WALL");
        if (col.gameObject.name == "Obstacle2")
        {
            // We need to find out which sphere collider we are hitting here
            Debug.Log("IN COLLISION");
        }
    }

    // Update is called once per frame
    void Update () {

        // We calculate the speed of the car for the collision force
        carSpeed = carModel.velocity.magnitude;
    }


    // COLLISION DETECTION
    //void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log("COLLISION WITH WALL");
    //    if (col.gameObject.name == "Obstacle2")
    //    {
    //        // We need to find out which sphere collider we are hitting here
    //        Debug.Log("IN COLLISION");
    //        // We make the calculation of the damage created by the collision
    //        // Force = (mass * speed * speed)/2
    //        damageCaused = (pieceMass * col.relativeVelocity.magnitude * col.relativeVelocity.magnitude) / 2;
    //        Debug.Log("DAMAGE: " + damageCaused);

    //        pieceHealth -= damageCaused;
    //        // The total slider is reduced in a smallest amount
    //        healthSlider.value = damageCaused * 0.1f;

    //        // If the piece hasn´t left health we detach the piece
    //        if (pieceHealth < 0)
    //        {
    //            Debug.Log("Detach part");
    //            //isHanging = true;
    //        }

    //        // We check which part we´ve collided with

    //        // We check the health of the part to see if we detach it

    //        // everything affects the health of the car
    //    }
    //}
}
