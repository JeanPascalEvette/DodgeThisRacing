using UnityEngine;
using System.Collections;

public class WheelController : MonoBehaviour {

    [SerializeField]
    private float mFrictionCoef = 1.0f;

    private CarController mCarController;

	// Use this for initialization
	void Start () {
        mCarController = transform.parent.GetComponent<CarController>();

    }
	
	// Update is called once per frame
	void Update () {
	}

    public float GetFrictionLimit()
    {
        return mFrictionCoef * mCarController.GetMassOnAxle(transform.localPosition.z);
    }
}
