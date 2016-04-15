using UnityEngine;
using System.Collections;

public class BoundaryDestroyer : MonoBehaviour {

    private GameLogic GL;

	// Use this for initialization
	void Start () {
        GL = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("coliision has occured");

        if (other.gameObject.tag == "Player")
        {
            var pData = other.gameObject.GetComponent<CarController>().myPlayerData;
          //  GL = GetComponent<GameLogic>();
            GL.DestroyCar(pData);
            StartCoroutine(RespawnCar(pData));
        }
    }

    IEnumerator RespawnCar(PlayerData car)
    {
        yield return new WaitForSeconds(1);
        GL.SpawnCar(car);
    }
}
