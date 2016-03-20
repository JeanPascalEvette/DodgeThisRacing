using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DetachableElementBehaviour : MonoBehaviour {

    public bool isHanging = false;
    public float timerBreak = -1f;
    public float timeToBreak = 5.0f;

    public float pieceHealth;
    public float pieceMass;

    [SerializeField]
    private GameObject DebrisHolder;
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        if (DebrisHolder == null)
            DebrisHolder = GameObject.Find("DebrisHolder");
        if (DebrisHolder == null)
            DebrisHolder = new GameObject("DebrisHolder");

        setColliders(false);
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;

    }
	
    private void setColliders(bool isEnabled)
    {
        foreach (BoxCollider col in GetComponents<BoxCollider>())
            col.enabled = isEnabled;
        foreach (BoxCollider col in GetComponentsInChildren<BoxCollider>())
            col.enabled = isEnabled;
    }


    void LateUpdate()
    {
        if (!isHanging)
        {
            transform.localRotation = initialRotation;
            transform.localPosition = initialPosition;
        }
    }
    // Update is called once per frame
    void Update() {
        if (isHanging && timerBreak == -1f)
        {
            timerBreak = timeToBreak;
            rb.mass = 1;
            setColliders(true);
            rb.constraints = RigidbodyConstraints.None;
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
                transform.parent = DebrisHolder.transform;
            }
            else if (timerBreak < timeToBreak / 2)
            {
                isHanging = true;
            }
        }
	}

}
