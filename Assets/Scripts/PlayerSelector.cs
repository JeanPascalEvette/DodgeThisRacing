using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{

    public Text t;
    int switch_case = -1;
    public int PanelNumber;
    string nameplayer;

    void Start() {

        switch (PanelNumber) {

            case 1: nameplayer = "P1";
                    break;
            case 2:
                    nameplayer = "P2";
                    break;
            case 3:
                    nameplayer = "P3";
                    break;
            case 4:
                    nameplayer = "P4";
                    break;
        }

        player_selector();

    }

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
                    t.text = nameplayer;
                    break;
                case 2:
                    //Debug.Log("cpu mode selected");
                    t.text = "CPU";
                    break;
                default:
                    //Debug.Log("null mode selected");
                    t.text = "None";
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
