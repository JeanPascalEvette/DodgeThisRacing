using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector3 : MonoBehaviour
{

    public Text t;
    int switch_case = 0;

    void switch_reset()
    {
        switch_case = 0;
    }

    public void player_selector()
    {

        switch_case++;

        if (switch_case < 4)
        {
            switch (switch_case)
            {
                case 1:
                    //Debug.Log("player mode selected");
                    t.text = "P1";
                    break;
                case 2:
                    //Debug.Log("cpu mode selected");
                    t.text = "CPU";
                    break;
                default:
                    //Debug.Log("null mode selected");
                    t.text = " ";
                    switch_case = 0;
                    break;
            }
            Debug.Log(switch_case);
        }

        else
        {
            Debug.Log(switch_case);
            switch_reset();
        }

    }
}
