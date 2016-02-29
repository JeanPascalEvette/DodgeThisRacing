using UnityEngine;
using System.Collections;

public class DetachableElementBehaviour : MonoBehaviour {

    public bool isHanging = false;
    public float timerBreak = -1f;
    public float timeToBreak = 5.0f;

    [SerializeField]
    private Transform DebrisHolder;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(isHanging && timerBreak == -1f)
        {
            rb.constraints = RigidbodyConstraints.None;
            timerBreak = timeToBreak;
        }
        else if(isHanging)
        {
            timerBreak -= Time.deltaTime;
            if(timerBreak <= 0.0f)
            {
                SpringJoint[] components = transform.GetComponents<SpringJoint>();
                foreach (SpringJoint sj in components)
                    Destroy(sj);
                transform.parent = DebrisHolder;
            }
        }
	}
}
