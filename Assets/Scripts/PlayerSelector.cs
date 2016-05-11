using UnityEngine;
using UnityEngine.UI;

//This script is attached to each panel controlling the status of a player and its control types
public class PlayerSelector : MonoBehaviour
{
    public Text t;
    public Text cpuText;
    public Text Control_Type;

    public int switch_case = 0;
    public int Controls = 0;
    public int CPU_Controls = -1;
    public int PanelNumber;

    public GameObject playerCoin;
    public IconCollider Car1, Car2, Car3, Car4;
    public LevelManager l;
    public MoveSelector m;

    string nameplayer;
    public bool is_CPU = false;

    public Sprite default_Empty, CurrentCar,hand_closed,hand_opened;

    private Image carImage;
    public Image Hand;

    private ButtonController buttonController;

    void Start() {

        buttonController = GetComponentInChildren<ButtonController>();

        l.is_p1_active = true;

        carImage = transform.FindChild("CarImage").GetComponent<Image>();
        carImage.sprite = default_Empty;


        switch (PanelNumber)
        {

            case 1:
                if (l.is_p1_active == true) { nameplayer = "1P"; Controls = 1; }
                break;

            case 2:
                nameplayer = "2P"; 
                break;

            case 3:
                nameplayer = "3P"; 
                break;

            case 4:
                nameplayer = "4P"; 
                break;
        }

        player_selector();

        if (m.is_this_active)
        {
            if      (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)      { Control_Type.text = "Joy1"; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)      { Control_Type.text = "Joy2"; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { Control_Type.text = "ArrowKeys"; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)      { Control_Type.text = "WSDA"; }
            else                                                                     { Control_Type.text = "Not Assigned"; }
        }

        else { Control_Type.text = "Not Assigned"; }

    }

    void Update()
    {

        if (m.is_this_active)
        {

            if      (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)      { Control_Type.text = "Joy1"; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)      { Control_Type.text = "Joy2"; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { Control_Type.text = "ArrowKeys"; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)      { Control_Type.text = "WSDA"; }

            if      (PanelNumber == 1 && switch_case != 2 && !is_CPU)                { t.text = "P1"; }
            else if (PanelNumber == 2 && switch_case != 2 && !is_CPU)                { t.text = "P2"; }
            else if (PanelNumber == 3 && switch_case != 2 && !is_CPU)                { t.text = "P3"; }
            else if (PanelNumber == 4 && switch_case != 2 && !is_CPU)                { t.text = "P4"; }
        }

        else { Control_Type.text = "Not Assigned"; }
    }

    void switch_reset() { switch_case = 0; }

    public void player_selector()
    {
        if (switch_case < 4)
        {
            switch (switch_case)
            {
                case 1:
                    if (m.is_this_active == true) { t.text = nameplayer; }
                    else                          { t.text = "N/A"; }
                    break;

                case 2:
                    t.text = "CPU";
                    break;

                default:
                    t.text = "N/A";
                    switch_case = 0;
                    break;
            }
        }

        else { switch_reset(); }
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

                        if (l.num_players < m.playerID) { l.num_players++; }

                        if      (m.playerID == 1) { l.is_p1_active = true; }
                        else if (m.playerID == 2) { l.is_p2_active = true; }
                        else if (m.playerID == 3) { l.is_p3_active = true; }
                        else if (m.playerID == 4) { l.is_p4_active = true; }

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

                        if      (m.playerID == 1) { l.is_p1_active = false; }
                        else if (m.playerID == 2) { l.is_p2_active = false; }
                        else if (m.playerID == 3) { l.is_p3_active = false; }
                        else if (m.playerID == 4) { l.is_p4_active = false; }

                        l.num_players--;

                        m.is_this_active = false;
                        is_CPU = false;
                    }

                    switch_case = 0;
                    playerCoin.transform.position = m.playerPosition;

                    if      (Car1.ThisCarSelected == true && m.playerID == Car1.ID) { Car1.CheckPlayerActivation();}
                    else if (Car2.ThisCarSelected == true && m.playerID == Car2.ID) { Car2.CheckPlayerActivation();}
                    else if (Car3.ThisCarSelected == true && m.playerID == Car3.ID) { Car3.CheckPlayerActivation();}
                    else if (Car4.ThisCarSelected == true && m.playerID == Car4.ID) { Car4.CheckPlayerActivation();}

                    CPU_Controls = 0;
                    t.text = "N/A";
                    cpuText.text = nameplayer;

                    buttonController.SetOverlay(false);

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

    public void ImageSwapper() {

        //this.gameObject.GetComponent<Image>().sprite = CurrentCar;
        carImage.sprite = CurrentCar;
    }

    public void ControlManager() {

        if (m.is_this_active && !m.is_this_ready)
        {
            Controls++;

            switch (Controls)
            {
                case 1:

                    if (l.is_joy1_taken == false) 
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
                        l.is_joy1_taken = true;
                        Control_Type.text = "Joy1";
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1) { }
                    else {
                           m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                           Control_Type.text = "Joy1 (N/A)";
                         }

                    break;

                case 2:
                    if (l.is_joy2_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy2;
                        l.is_joy2_taken = true;
                        Control_Type.text = "Joy2";
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2) { }
                    else {
                           m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                           Control_Type.text = "Joy2 (N/A)";
                         }
                    break;

                case 3:
                    if (l.is_arrowKeys_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.ArrowKeys;
                        l.is_arrowKeys_taken = true;
                        Control_Type.text = "ArrowKeys";
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { }
                    else {
                           m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                           Control_Type.text = "ArrowKeys (N/A)";
                         }
                    break;

                case 4:
                    if (l.is_wsda_taken == false)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.WSDA;
                        l.is_wsda_taken = true;
                        Control_Type.text = "WSDA";
                    }

                    else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA) { }
                    else {
                           m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                           Control_Type.text = "WSDA (N/A)";
                         }
                    break;

                default:
                   m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                   Control_Type.text = "Not Assigned";
                   Controls = 0;
                   break;
            }
        }
    }
}
