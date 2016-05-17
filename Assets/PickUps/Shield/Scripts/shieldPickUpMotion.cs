using UnityEngine;
using System.Collections;

public class shieldPickUpMotion : MonoBehaviour {

    [SerializeField]
    float floatSpeed;

    float y0;

    [SerializeField]
    float amplitude;

    // Use this for initialization
    void Start()
    {
        y0 = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        this.GetComponent<Transform>().position = new Vector3 (transform.position.x, y0 + amplitude * Mathf.Sin(floatSpeed * Time.time), transform.position.z);

    }
}
