using UnityEngine;
using System.Collections;

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
