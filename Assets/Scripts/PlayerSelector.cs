using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{

    public Text t;
    public Text Control_Type;
    public int switch_case = 0;
    public int Controls = 0;
    public int CPU_Controls = -1;
    public int PanelNumber;
    string nameplayer;
    public GameObject playerCoin;

    public IconCollider Car1, Car2, Car3, Car4;

    bool is_CPU = false;

    public LevelManager l;
    public MoveSelector m;

    public Text cpuText;

    void Start() {

        // m = playerCoin.GetComponent<MoveSelector>();
        l.is_p1_active = true;

        switch (PanelNumber)
        {

            case 1:

                if (l.is_p1_active == true) { nameplayer = "1P"; Controls = 1; }
                //else                        { t.text = "N/A"; }
                break;

            case 2:
                /*if (l.is_p2_active == true)*/ { nameplayer = "2P"; }
               // else                        { t.text = "N/A"; }
                
                break;

            case 3:
                /*if (l.is_p3_active == true)*/ { nameplayer = "3P"; }
                //else                        { t.text = "N/A"; }
                break;

            case 4:
                /*if (l.is_p4_active == true)*/ { nameplayer = "4P"; }
                //else                          { t.text = "N/A"; }

                break;
        }

        player_selector();
        PanelManager();


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



            if      (PanelNumber == 1 && switch_case != 2 && !is_CPU) { t.text = "P1"; }
            else if (PanelNumber == 2 && switch_case != 2 && !is_CPU) { t.text = "P2"; }
            else if (PanelNumber == 3 && switch_case != 2 && !is_CPU) { t.text = "P3"; }
            else if (PanelNumber == 4 && switch_case != 2 && !is_CPU) { t.text = "P4"; }

        }

        else { Control_Type.text = "Not Assigned"; }

        

       





    }

    void switch_reset()
    {
        switch_case = 0;
    }

    public void player_selector()
    {

       

        if (switch_case < 4 /*&& l.num_players == m.playerID - 1*/)
        {
            switch (switch_case)
            {
                case 1:
                    //Debug.Log("player mode selected");
                    if (m.is_this_active == true) { t.text = nameplayer; }
                    else                          { t.text = "N/A"; }
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
        if (l.num_players == m.playerID || l.num_players == m.playerID - 1)
        {
            switch_case++;
            switch (switch_case)
            {
                case 1:

                    if (l.num_players == m.playerID - 1 && !m.is_this_active)
                    {

                        playerCoin.SetActive(true);
                        cpuText.text = nameplayer;
                        t.text = nameplayer;

                        playerCoin.transform.position = m.playerPosition;//check this

                        //t.text = nameplayer;

                        if (l.num_players < m.playerID) { l.num_players++; }
                        //l.num_active++;

                        if      (m.playerID == 1) { l.is_p1_active = true; }
                        else if (m.playerID == 2) { l.is_p2_active = true; }
                        else if (m.playerID == 3) { l.is_p3_active = true; }
                        else if (m.playerID == 4) { l.is_p4_active = true; }

                        //m = playerCoin.GetComponent<MoveSelector>();
                        m.is_this_active = true;
                        CPU_Controls = 1;
                        is_CPU = false;
                    }

                   

                    break;

                case 2:


                    if (l.num_players == m.playerID - 1 && !m.is_this_active)
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
                        is_CPU = true;
                        CPU_Controls = 2;
                    }



                    break;

                default:

                    if (l.num_players == m.playerID && m.is_this_active)

                    {
                        print("deleting player");

                        playerCoin.SetActive(false);

                        if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
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

                        if (m.playerID == 1) { l.is_p1_active = false; }
                        else if (m.playerID == 2) { l.is_p2_active = false; }
                        else if (m.playerID == 3) { l.is_p3_active = false; }
                        else if (m.playerID == 4) { l.is_p4_active = false; }

                        l.num_players--;

                        m.is_this_active = false;
                        is_CPU = false;


                    }

                    switch_case = 0;

                    if      (Car1.ThisCarSelected == true && m.playerID == Car1.ID) { Car1.CheckPlayerActivation();}
                    else if (Car2.ThisCarSelected == true && m.playerID == Car2.ID) { Car2.CheckPlayerActivation();}
                    else if (Car3.ThisCarSelected == true && m.playerID == Car3.ID) { Car3.CheckPlayerActivation();}
                    else if (Car4.ThisCarSelected == true && m.playerID == Car4.ID) { Car4.CheckPlayerActivation();}

                    CPU_Controls = 0;
                    t.text = "N/A";

                    break;
            }
        }

        else if(m.is_this_active)
        {

            CPU_Controls++;

            switch (CPU_Controls)
            {

                case 1:
                    cpuText.text = nameplayer;
                    t.text = nameplayer;
                    is_CPU = false;
                    switch_case = 1;
                    break;

                case 2:
                    cpuText.text = "CPU";
                    t.text = "CPU";
                    is_CPU = true;
                    CPU_Controls = 0;
                    switch_case = 2;

                    break;

                default:
                    switch_case = 0;
                    CPU_Controls = 0;
                    break;
            }

        }
    }


    public void ControlManager() {

        print(Controls);

        if (m.is_this_active)
        {

            Controls++;

            switch (Controls)
            {

                case 1:

                    if (/*m.ThisPlayerControl != MoveSelector.ControlTypesHere.Joy1 &&*/ l.is_joy1_taken == false) // first statement of if not really needed
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
                        l.is_joy1_taken = true;
                        Control_Type.text = "Joy1";
                    }
                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1) { }
                    else { m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned; Control_Type.text = "Joy1 (N/A)"; }

                    break;

                case 2:
                    if (/*m.ThisPlayerControl != MoveSelector.ControlTypesHere.Joy2 &&*/ l.is_joy2_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy2;

                        l.is_joy2_taken = true;
                        Control_Type.text = "Joy2";
                    
                    }
                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2) { }
                    else { m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned; Control_Type.text = "Joy2 (N/A)"; }
                    break;

                case 3:
                    if (/*m.ThisPlayerControl != MoveSelector.ControlTypesHere.ArrowKeys &&*/ l.is_arrowKeys_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.ArrowKeys;



                        l.is_arrowKeys_taken = true;
                        Control_Type.text = "ArrowKeys";
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { }
                    else { m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned; Control_Type.text = "ArrowKeys (N/A)"; }
                    break;

                case 4:
                    if (/*m.ThisPlayerControl != MoveSelector.ControlTypesHere.WSDA &&*/ l.is_wsda_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.WSDA;
                        l.is_wsda_taken = true;
                        Control_Type.text = "WSDA";
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA) { }
                    else { m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned; Control_Type.text = "WSDA (N/A)"; }
                    break;

                default:
                   // if (m.ThisPlayerControl != MoveSelector.ControlTypesHere.NotAssigned)
                   // {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;

                       Control_Type.text = "Not Assigned";
                   // }
                    Controls = 0;
                    break;


            }

            

        }
    }
}
