using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
    public bool healthPickup;
    public bool lifePickup;
    public bool speedPickup;
    public bool shieldPickup;
    public bool shieldDome;

    public float health = 50f;
    public float life = 1f;
    public float speed = 5f;
    public float shield = 5f;
}
