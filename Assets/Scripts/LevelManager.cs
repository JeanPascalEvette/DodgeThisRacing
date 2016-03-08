using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Player p;
    public Text TextColor;

    void Start()
    {
        TextColor = GameObject.FindWithTag("Go").GetComponent<Text>();
        p = GameObject.FindWithTag("PlayerMenu").GetComponent<Player>();
    }


    void Update()
    {
        if (p.notSelected == false)
        {
            TextColor.color = Color.green;

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
}