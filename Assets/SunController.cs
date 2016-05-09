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
        {
            foreach(var pd in Data.GetPlayerData())
            {
                var car = pd.GetGameObject();
                if (car != null)
                    car.GetComponent<CarController>().SetLights(true);
            }
        }
        else
        {
            foreach (var pd in Data.GetPlayerData())
            {
                var car = pd.GetGameObject();
                if (car != null)
                    car.GetComponent<CarController>().SetLights(false);
            }
        }
    }
}
