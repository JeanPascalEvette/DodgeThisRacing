using UnityEngine;
using System.Collections;

public class shieldCollisionEffect : MonoBehaviour {

    Light collisionLight;
    [SerializeField]
    float fadeSpeed = 0.5f;

	// Use this for initialization
	void Start () {

        collisionLight = this.GetComponentInChildren<Light>();
        collisionLight.range = 5;
	
	}
	
	// Update is called once per frame
	void Update () {

        collisionLight.range = collisionLight.range - Time.deltaTime * fadeSpeed;

        if (collisionLight.range < 0.01f)
        {
            Destroy(this.gameObject);
        }
	
	}
}
