using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerSelector : MonoBehaviour {

    public void LoadLevel(string name)
    {

        SceneManager.LoadScene(name);

    }


    //public GameObject player;
    //public void ShowPlayer(int value)
    //{
    //    player = GameObject.FindWithTag("PlayerMenu");
    //    player.SetActive(false);


    //}


}
