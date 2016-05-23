using UnityEngine;
using System.Collections;


//This monobehaviour destroys the objects after a set time. Used for ephemeral effects (i.e. sparks)
public class GameObjectCleaner : MonoBehaviour {

    [SerializeField]
    private float despawnTime;
    private float lifeTime;
	// Use this for initialization
	void Start () {
        lifeTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;
        if (despawnTime < lifeTime)
            Destroy(this.gameObject);
	}
}
