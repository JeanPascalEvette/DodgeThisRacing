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
        if (DebrisHolder == null)
            DebrisHolder = GameObject.Find("DebrisHolder").transform;
        setColliders(false);

    }
	
    private void setColliders(bool isEnabled)
    {
        foreach (BoxCollider col in GetComponents<BoxCollider>())
            col.enabled = isEnabled;
        foreach (BoxCollider col in GetComponentsInChildren<BoxCollider>())
            col.enabled = isEnabled;
    }

    // Update is called once per frame
    void Update() {
        if (isHanging && timerBreak == -1f)
        {
            timerBreak = timeToBreak;
            rb.constraints = RigidbodyConstraints.None;
            rb.mass = 1;
            setColliders(true);
        }
        else if(isHanging)
        {
            timerBreak -= Time.deltaTime;
            if (timerBreak <= 0.0f)
            {
                SpringJoint[] components = transform.GetComponents<SpringJoint>();
                foreach (SpringJoint sj in components)
                    Destroy(sj);
                HingeJoint[] hjs = transform.GetComponents<HingeJoint>();
                foreach (HingeJoint hj in hjs)
                    Destroy(hj);
                FixedJoint[] fjs = transform.GetComponents<FixedJoint>();
                foreach (FixedJoint fj in fjs)
                    Destroy(fj);
                transform.parent = DebrisHolder;
            }
            else if (timerBreak < timeToBreak / 2)
            {

            }
        }
	}
}
