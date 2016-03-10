using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerSelector : MonoBehaviour {

    public GameObject p1, p2, p3, p4;
    public Text t;
    int switch_case = 1;
    int pl = 0;

    public void this_button()
    {
    
      
        
        //p3 = GameObject.FindWithTag("p3");
        //p4 = GameObject.FindWithTag("p4");

    }



    public void player_selector()
    {
        //p1 = GameObject.FindWithTag("p");
        //t1 = GameObject.FindWithTag("t").GetComponent<Text>();

        

        switch_case ++;
        switch (switch_case)
        {
            case 1:
                Debug.Log("player mode selected");
                t.text = "P1";
                break;
            case 2:
                Debug.Log("cpu mode selected");
                t.text = "CPU";
                break;
            default:
                Debug.Log("null mode selected");
                t.text = " ";
                switch_case = 0;
                break;
        }
    }
}
