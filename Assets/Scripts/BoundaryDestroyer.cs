using UnityEngine;
using System.Collections;


//This class handles the invisible boundary which pushes car to keep up with the race, and also damages the ones that fall behind.
public class BoundaryDestroyer : MonoBehaviour {

    private GameLogic GL;
    private Quaternion OrgRotation;
    private Vector3 OrgPosition;

    [SerializeField]
    private float lifeDecreaseSpeed = 10.0f;

    
    float boostDistMax = 40.0f;
    float boostDistMin = 10.0f;
    public float lifeLossDistMin = 18.0f;

    // Use this for initialization
    void Start () {
        GL = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
    

    void Awake()
    {
        OrgRotation = transform.rotation;
        OrgPosition = transform.position;
    }

    //The boundaryDestroyer is a child of the camera. This makes sure that it will not be affected by the rotation of the camera in case of zoomout/zoomin
    void LateUpdate()
    {
        transform.rotation = OrgRotation;
        transform.position = OrgPosition + new Vector3(0,0,transform.parent.position.z - UnityEngine.Camera.main.GetComponent<CameraController>().getZoomZDiff());
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (Data.GetPlayerData() == null) return;
        var leadingCar = UnityEngine.Camera.main.GetComponent<CameraController>().GetLeadingPlayerPosition();
        leadingCar.z -= UnityEngine.Camera.main.GetComponent<CameraController>().getZoomZDiff() + 20.0f;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(leadingCar - new Vector3(20, 0, 0), leadingCar + new Vector3(20, 0, 0));
    }
#endif

    //Returns the z coordinate of the pushing wall based on leading car.
    public float GetPushingWall()
    {
        var leadingCar = UnityEngine.Camera.main.GetComponent<CameraController>().GetLeadingPlayerPosition().z;
        leadingCar -= UnityEngine.Camera.main.GetComponent<CameraController>().getZoomZDiff() + lifeLossDistMin + 5.0f;
        return leadingCar;
    }

    //In update we move the wall to follow the leading car, but also affect any car that is being left behind to help it get back in the race.
    void Update()
    {
        foreach(var pd in Data.GetPlayerData())
        {
            var car = pd.GetGameObject();
            if (car == null) continue;

            var leadingCar = UnityEngine.Camera.main.GetComponent<CameraController>().GetLeadingPlayerPosition();
            leadingCar.z -= UnityEngine.Camera.main.GetComponent<CameraController>().getZoomZDiff();

            //If very far behind wall just kill the car
            if (car.transform.position.z < leadingCar.z - 200.0f)
                GL.DestroyCar(pd, true);
            else if (car.transform.position.z < leadingCar.z - lifeLossDistMin) //If behind wall damage car progressively
                car.GetComponent<CarController>().ReduceLife(Time.deltaTime * lifeDecreaseSpeed);


            //If close to the wall - boost car a bit
            if (car.transform.position.z < leadingCar.z - boostDistMin)
            {
                float boost = 0.0f;
                float dist = leadingCar.z - car.transform.position.z;
                if (dist >= boostDistMax)
                    boost = 1.0f;
                else
                if (dist <= boostDistMin)
                    boost = 0.0f;
                else
                {
                    boost = (dist - boostDistMin) / (boostDistMax - boostDistMin);
                }
                car.GetComponent<CarController>().SetBoost(boost);

            }
            else
                car.GetComponent<CarController>().SetBoost(-1);
        }
    }

    IEnumerator RespawnCar(PlayerData car)
    {
        yield return new WaitForSeconds(1);
        GL.SpawnCar(car);
    }
}
