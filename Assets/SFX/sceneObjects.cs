using UnityEngine;
using System.Collections;

public class sceneObjects : MonoBehaviour {
    AudioSource audio;
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
       audio.Play();
    }
}
