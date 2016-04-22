using UnityEngine;
using System.Collections;

public class BoundaryDestroyer : MonoBehaviour {

    private GameLogic GL;
    private Quaternion OrgRotation;
    private Vector3 OrgPosition;
 

    // Use this for initialization
    void Start () {
        GL = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
    
    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            var pData = other.gameObject.GetComponent<CarController>().myPlayerData;
          //  GL = GetComponent<GameLogic>();
            GL.DestroyCar(pData);
            StartCoroutine(RespawnCar(pData));
        }
    }

    void Awake()
    {
        OrgRotation = transform.rotation;
        OrgPosition = transform.position;
    }

    void LateUpdate()
    {
        transform.rotation = OrgRotation;
        transform.position = OrgPosition + new Vector3(0,0,transform.parent.position.z - UnityEngine.Camera.main.GetComponent<Camera>().getZoomZDiff());
    }

    void Update()
    {

    }

    IEnumerator RespawnCar(PlayerData car)
    {
        yield return new WaitForSeconds(1);
        GL.SpawnCar(car);
    }
}
