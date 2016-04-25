using UnityEngine;
using System.Collections;

public class BoundaryDestroyer : MonoBehaviour {

    private GameLogic GL;
    private Quaternion OrgRotation;
    private Vector3 OrgPosition;

    [SerializeField]
    private float lifeDecreaseSpeed = 10.0f;

    // Use this for initialization
    void Start () {
        GL = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
    
    void OnCollisionEnter(Collision other)
    {
        return;
        if (other.gameObject.tag == "Player")
        {
            var pData = other.gameObject.GetComponent<CarController>().myPlayerData;
          //  GL = GetComponent<GameLogic>();
            GL.DestroyCar(pData, true);
            //StartCoroutine(RespawnCar(pData));
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
        foreach(var pd in Data.GetPlayerData())
        {
            if (pd.GetGameObject() != null && pd.GetGameObject().transform.position.z < transform.position.z - 200.0f)
                GL.DestroyCar(pd, true);
            else if (pd.GetGameObject() != null && pd.GetGameObject().transform.position.z < transform.position.z)
                pd.GetGameObject().GetComponent<CarController>().ReduceLife(Time.deltaTime * lifeDecreaseSpeed);
        }
    }

    IEnumerator RespawnCar(PlayerData car)
    {
        yield return new WaitForSeconds(1);
        GL.SpawnCar(car);
    }
}
