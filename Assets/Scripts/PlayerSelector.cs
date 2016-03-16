using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{

    public Text t;
    public Text Control_Type;
    int switch_case = 0;
    int Controls = 0;
    public int PanelNumber;
    string nameplayer;
    public GameObject playerCoin;

    public LevelManager l;
    public MoveSelector m;

    public Text cpuText;

    void Start() {

       // m = playerCoin.GetComponent<MoveSelector>();

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


        if (m.is_this_active)
        {

            if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
            {

                Control_Type.text = "Joy1";
            }

            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
            {

                Control_Type.text = "Joy2";
            }

            else if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.ArrowKeys)
            {

                Control_Type.text = "ArrowKeys";
            }

            else if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.WSDA)
            {

                Control_Type.text = "WSDA";
            }

            else { Control_Type.text = "Not Assigned"; }

        }

        else { Control_Type.text = "Not Assigned"; }

    }

    void Update() {

        if (m.is_this_active)
        {

            if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
            {

                Control_Type.text = "Joy1";
            }

            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
            {

                Control_Type.text = "Joy2";
            }

            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
            {

                Control_Type.text = "ArrowKeys";
            }

            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
            {

                Control_Type.text = "WSDA";
            }

            else { Control_Type.text = "Not Assigned"; }

        }

        else { Control_Type.text = "Not Assigned"; }

    }

    void switch_reset()
    {
        switch_case = 0;
    }

    public void player_selector()
    {

        switch_case++;

        if (switch_case < 4 /*&& l.num_players == m.playerID - 1*/)
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

                if (l.num_players == m.playerID - 1 && !m.is_this_active)
                {
                    playerCoin.SetActive(true);
                    cpuText.text = nameplayer;
                    t.text = nameplayer;
                    //t.text = nameplayer;

                    if (l.num_players < m.playerID) { l.num_players++; }
                    //l.num_active++;

                    if      (m.playerID == 1) { l.is_p1_active = true; }
                    else if (m.playerID == 2) { l.is_p2_active = true; }
                    else if (m.playerID == 3) { l.is_p3_active = true; }
                    else if (m.playerID == 4) { l.is_p4_active = true; }

                    //m = playerCoin.GetComponent<MoveSelector>();
                    m.is_this_active = true;
                }

                //else { }

                break;

            case 2:

               
                    if (l.num_players == m.playerID -1 && !m.is_this_active )
                    {
                        playerCoin.SetActive(true);
                        m.is_this_active = true;
                        l.num_players++;

                        if      (m.playerID == 1) { l.is_p1_active = true; }
                        else if (m.playerID == 2) { l.is_p2_active = true; }
                        else if (m.playerID == 3) { l.is_p3_active = true; }
                        else if (m.playerID == 4) { l.is_p4_active = true; }
                    }

                    if (m.is_this_active)
                     {
                         cpuText.text = "CPU";
                         t.text = "CPU";
                     }
                    
                

                break;

            default:

                if (l.num_players == m.playerID && m.is_this_active)

                {
                    print("deleting player");

                    playerCoin.SetActive(false);

                    if  (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        l.is_joy1_taken = false;
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        l.is_joy2_taken = false;
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        l.is_arrowKeys_taken = false;
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        l.is_wsda_taken = false;
                    }

                    if      (m.playerID == 1) { l.is_p1_active = false; }
                    else if (m.playerID == 2) { l.is_p2_active = false; }
                    else if (m.playerID == 3) { l.is_p3_active = false; }
                    else if (m.playerID == 4) { l.is_p4_active = false; }

                    l.num_players--;

                    m.is_this_active = false;
                    switch_case = 0;

                }

                t.text = "N/A";

                break;
        }

    }


    public void ControlManager() {

        if (m.is_this_active)
        {

            Controls++;

            switch (Controls)
            {

                case 1:
                    if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.Joy1 && l.is_joy1_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
                        l.is_joy1_taken = true;
                        Control_Type.text = "Joy1";
                    }

                   // else if(l.is_joy1_taken == true) { Control_Type.text = "Joy1 (N/A)"; }

                    break;

                case 2:
                    if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.Joy2 && l.is_joy2_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy2;



                        l.is_joy2_taken = true;
                        Control_Type.text = "Joy2";
                    
                    }
                    //else { Control_Type.text = "Joy2 (N/A)"; }
                    break;

                case 3:
                    if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.ArrowKeys && l.is_arrowKeys_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.ArrowKeys;



                        l.is_arrowKeys_taken = true;
                        Control_Type.text = "ArrowKeys";
                    }
                   // else { Control_Type.text = "ArrowKeys (N/A)"; }
                    break;
                case 4:
                    if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.WSDA && l.is_wsda_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.WSDA;



                        l.is_wsda_taken = true;
                        Control_Type.text = "WSDA";
                    }

                   // else { Control_Type.text = "WSDA (N/A)"; }
                    break;

                default:
                    if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.NotAssigned)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;



                        Control_Type.text = "Not Assigned";
                    }
                    Controls = 0;
                    break;


            }

            

        }
    }
}
