using UnityEngine;
using System.Collections;

public class healthUpRotate : MonoBehaviour {

    [SerializeField]
    float rotateSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);

    }
}
