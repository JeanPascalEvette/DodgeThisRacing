using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{

    public Text t;
    int switch_case = 0;
    public int PanelNumber;
    string nameplayer;
    public GameObject playerCoin;

    public LevelManager l;
    public MoveSelector m;

    public Text cpuText;

    void Start() {

        switch (PanelNumber)
        {

            case 1:

                /*if (l.is_p1_active == true) */{ nameplayer = "P1"; }
                
                break;

            case 2:
                /*if (l.is_p2_active == true)*/ { nameplayer = "P2"; }
                
                break;

            case 3:
                /*if (l.is_p3_active == true)*/ { nameplayer = "P3"; }
                
                break;

            case 4:
                /*if   (l.is_p4_active == true)*/ { nameplayer = "P4"; }
                
                break;
        }

        player_selector();

    }

    void Update() {



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

                    t.text = "N/A";
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

    public void PanelManager()
    {
        switch (switch_case)
        {
            case 1:

                if (l.num_players == m.playerID - 1)
                {
                    playerCoin.SetActive(true);
                    cpuText.text = nameplayer;
                    //t.text = nameplayer;

                    //l.num_players++;
                    //l.num_active++;

                    if      (m.playerID == 1) { l.is_p1_active = true; }
                    else if (m.playerID == 2) { l.is_p2_active = true; }
                    else if (m.playerID == 3) { l.is_p3_active = true; }
                    else if (m.playerID == 4) { l.is_p4_active = true; }
                }

                break;

            case 2:

                if (l.num_players == m.playerID )

                {
                    playerCoin.SetActive(true);
                    cpuText.text = "CPU";
                   // t.text = "CPU";
                }
                break;

            default:

                if (l.num_players == m.playerID)

                {
                    playerCoin.SetActive(false);

                    if      (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)      { l.is_joy1_taken = false; }
                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)      { l.is_joy2_taken = false; }
                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { l.is_arrowKeys_taken = false; }
                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)      { l.is_wsda_taken = false; }

                    if      (m.playerID == 1) { l.is_p1_active = false; }
                    else if (m.playerID == 2) { l.is_p2_active = false; }
                    else if (m.playerID == 3) { l.is_p3_active = false; }
                    else if (m.playerID == 4) { l.is_p4_active = false; }

                    l.num_players--;
                    switch_case = 0;

                    //t.text = "N/A";
                }

                break;
        }

    }
}
