using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Player p;
    public Text TextColorGo;
    public Text TextColorCar;
    bool is_inside = false;

    void Start()
    {
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();
        TextColorCar = GameObject.FindWithTag("Car").GetComponent<Text>();
        p = GameObject.FindWithTag("PlayerMenu").GetComponent<Player>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.X) && is_inside)
        {
            p.notSelected = false;
        }

        if (Input.GetKey(KeyCode.A) && is_inside && (!p.notSelected))
        {
            p.notSelected = true;
        }

        if (p.notSelected == false)
        {
            TextColorGo.color = Color.green;

            if (Input.GetKey(KeyCode.B))
            {

                LoadLevel(name);
            }
        }


    }

    public void LoadLevel(string name)
    {
        name = "main2";
        SceneManager.LoadScene(name);

    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        print("Trigger");

        TextColorCar.color = Color.yellow;
        is_inside = true;
    }



    void OnTriggerExit2D(Collider2D trigger)
    {
        print("EXIT");
        TextColorCar.color = Color.white;
        is_inside = false;
    }
}