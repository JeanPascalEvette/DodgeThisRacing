using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

    static MusicPlayer instance = null;
    //initialize to null because at first there will be no defined thing call instance

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    //Update is called once per frame
    void Update()
    {
       if(SceneManager.GetActiveScene().name == "Game")
        {
            Destroy(gameObject);
        }

    }
}