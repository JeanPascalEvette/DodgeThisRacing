using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class inGameGUI : MonoBehaviour {

    public Text score1;
    public Text score2;
    public Text score3;
    public Text score4;
    public Text middleText;
    public GameLogic gmlg;

    private float timer;
    private int rand;
    private bool countdown;
    private int x = 0;

    void Start () {
        score1.text = "";
        score2.text = "";
        score3.text = "";
        score4.text = "";
        middleText.text = "";
        rand = 0;
        timer = 0;
        countdown = false;
    }

	void Update () {
        UpdateScore();
        textPopups();

       
        if (countdown == true)
        {
            timer -= Time.deltaTime;

            if (gmlg.winner < 0 && timer <=0)
            {   
                countdown = false;
            }

            if (gmlg.winner >= 0 && timer <= 0)    
            {
                countdown = false;
            }
        }
        else
        {
            middleText.text = "";
        }
       
        

    }

    void UpdateScore()
    {
        if (Data.getNumberCarSelected() == 4)
        {
            score1.text = "Player 1 remaining lives: " + Data.GetPlayerData()[0].getLives();
            score2.text = "Player 2 remaining lives: " + Data.GetPlayerData()[1].getLives();
            score3.text = "Player 3 remaining lives: " + Data.GetPlayerData()[2].getLives();
            score4.text = "Player 4 remaining lives: " + Data.GetPlayerData()[3].getLives();
        }
        else if (Data.getNumberCarSelected() == 3)
        {
            score1.text = "Player 1 remaining lives: " + Data.GetPlayerData()[0].getLives();
            score2.text = "Player 2 remaining lives: " + Data.GetPlayerData()[1].getLives();
            score3.text = "Player 3 remaining lives: " + Data.GetPlayerData()[2].getLives();
        }
        else if (Data.getNumberCarSelected() == 2)
        {
            score1.text = "Player 1 remaining lives: " + Data.GetPlayerData()[0].getLives();
            score2.text = "Player 2 remaining lives: " + Data.GetPlayerData()[1].getLives();
        }
        else if (Data.getNumberCarSelected() == 1)
        {
            score1.text = "Player 1 remaining lives: " + Data.GetPlayerData()[0].getLives();
        }
    }

    void textPopups()
    {
        if (gmlg.winner >= 0 && countdown == false)
        {
            
            x++;
            if(x == 2)
            {
                SceneManager.LoadScene("menu");
            }
            timer = 7;
            countdown = true;
            rand = Random.Range(1, 8);
            switch (rand)
            {
                case 1:
                    middleText.text = "Player " + gmlg.winner + " has WON! I knew I should have bet on him...";
                    break;
                case 2:
                    middleText.text = "Player " + gmlg.winner + " is VICTORIOUS! All hail!";
                    break;
                case 3:
                    middleText.text = "Player " + gmlg.winner + " has WON! Cheer up Player "  +(gmlg.lastDeath) + ". You'll do better next time!";
                    break;
                case 4:
                    middleText.text = "Player " + gmlg.winner + " is the new champion! He will buy champagne for all!";
                    break;
                case 5:
                    middleText.text = "Player " + gmlg.winner + " has WON... and NO Player " + (gmlg.lastDeath)  + "... it wasn't a gameplay bug that caused you to lose!";
                    break;
                case 6:
                    middleText.text = "Player " + gmlg.winner + " has WON! It's not just about winning, it's about making others lose!";
                    break;
                case 7:
                    middleText.text = "Player " + gmlg.winner + " is VICTORIOUS! Nothing tastes better than the sweeeeet taste of victory!";
                    break;
            }
        }

        else if (Data.GetPlayerData()[gmlg.lastDeath - 1].getLives() == 0 && countdown == false)
        {
            
            timer = 2;
            rand = Random.Range(1, 9);
            countdown = true;
            switch (rand)
            {
                case 1:
                    middleText.text = "Player " + gmlg.lastDeath + " has been ELIMINATED!";
                    break;
                case 2:
                    middleText.text = "Gotta be faster than that Player " + gmlg.lastDeath;
                    break;
                case 3:
                    middleText.text = "Back to driving school Player " + gmlg.lastDeath;
                    break;
                case 4:
                    middleText.text = "Looks like you needed more than 10 lives Player " + gmlg.lastDeath;
                    break;
                case 5:
                    middleText.text = (Data.GetCountPlayersDead()) + " down " + ((Data.getNumberCarSelected() - Data.GetCountPlayersDead()) -1) + " to go!";
                    break;
                case 6:
                    middleText.text = "Too slow Player " + gmlg.lastDeath + "!";
                    break;
                case 7:
                    middleText.text = "We will develop a motorcycle for you next time Player " + gmlg.lastDeath;
                    break;
                case 8:
                    middleText.text = "Give the controller to someone else Player " + gmlg.lastDeath + "!";
                    break;
                case 9:
                    middleText.text ="Player " + gmlg.lastDeath + " has been ANNIHILATED!";
                    break;
            }
            gmlg.lastDeath = Data.getNumberCarSelected();
            rand = 0;
        }
    }
}
