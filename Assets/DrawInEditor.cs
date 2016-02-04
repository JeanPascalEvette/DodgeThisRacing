using UnityEngine;
using System.Collections;

public class DrawInEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
	}
}
