using UnityEngine;
using System.Collections;

public class shieldCollisionEffect : MonoBehaviour {

    Light collisionLight;
    [SerializeField]
    float fadeSpeed = 0.5f;

	// Use this for initialization
	void Start () {

        collisionLight = this.GetComponentInChildren<Light>();
	
	}
	
	// Update is called once per frame
	void Update () {

        collisionLight.range = collisionLight.range - Time.time * fadeSpeed;

        if (collisionLight.range < 0.01f)
        {
            Destroy(this.gameObject);
        }
	
	}
}
