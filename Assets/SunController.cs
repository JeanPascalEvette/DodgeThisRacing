using UnityEngine;
using System.Collections;

public class SunController : MonoBehaviour {

    [SerializeField]
    private float rotateSpeed;

    public bool lightsOn;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Time.deltaTime * rotateSpeed, 0, 0);
        if (transform.rotation.eulerAngles.x > 270 || transform.rotation.eulerAngles.x < 20)
            lightsOn = true;
        else
            lightsOn = false;

    }
}
