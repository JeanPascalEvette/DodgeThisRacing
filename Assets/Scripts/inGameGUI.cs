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

            if (gmlg.victory == false && timer <=0)
            {   
                countdown = false;
            }

            if (gmlg.victory == true && timer <= 0)    
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
        if (gmlg.NUMBEROFCARS < 5)
        {
            score1.text = "Player 1 remaining lives: " + gmlg.scoreCount[0];
            score2.text = "Player 2 remaining lives: " + gmlg.scoreCount[1];
            score3.text = "Player 3 remaining lives: " + gmlg.scoreCount[2];
            score4.text = "Player 4 remaining lives: " + gmlg.scoreCount[3];
        }
        else if (gmlg.NUMBEROFCARS < 4)
        {
            score1.text = "Player 1 remaining lives: " + gmlg.scoreCount[0];
            score2.text = "Player 2 remaining lives: " + gmlg.scoreCount[1];
            score3.text = "Player 3 remaining lives: " + gmlg.scoreCount[2];
        }
        else if (gmlg.NUMBEROFCARS < 3)
        {
            score1.text = "Player 1 remaining lives: " + gmlg.scoreCount[0];
            score2.text = "Player 2 remaining lives: " + gmlg.scoreCount[1];
        }
    }

    void textPopups()
    {
        if (gmlg.victory == true && countdown == false)
        {
            
            x++;
            if(x == 2)
            {
                SceneManager.LoadScene("start");
            }
            timer = 7;
            countdown = true;
            rand = Random.Range(1, 8);
            switch (rand)
            {
                case 1:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " has WON! I knew I should have bet on him...";
                    break;
                case 2:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " is VICTORIOUS! All hail!";
                    break;
                case 3:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " has WON! Cheer up Player "  +((gmlg.NUMBEROFCARS - gmlg.playerDeathNumber) + 1) + ". You'll do better next time!";
                    break;
                case 4:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " is the new champion! He will buy champagne for all!";
                    break;
                case 5:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " has WON... and NO Player " + ((gmlg.NUMBEROFCARS - gmlg.playerDeathNumber) + 1)  + "... it wasn't a gameplay bug that caused you to lose!";
                    break;
                case 6:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " has WON! It's not just about winning, it's about making others lose!";
                    break;
                case 7:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " is VICTORIOUS! Nothing tastes better than the sweeeeet taste of victory!";
                    break;
            }
        }

        else if (gmlg.scoreCount[gmlg.playerDeathNumber - 1] == 0 && countdown == false)
        {
            
            timer = 2;
            rand = Random.Range(1, 9);
            countdown = true;
            switch (rand)
            {
                case 1:
                    middleText.text = "Player " + gmlg.playerDeathNumber + " has been ELIMINATED!";
                    break;
                case 2:
                    middleText.text = "Gotta be faster than that Player " + gmlg.playerDeathNumber;
                    break;
                case 3:
                    middleText.text = "Back to driving school Player " + gmlg.playerDeathNumber;
                    break;
                case 4:
                    middleText.text = "Looks like you needed more than 10 lives Player " + gmlg.playerDeathNumber;
                    break;
                case 5:
                    middleText.text = (gmlg.winCondition +1) + " down " + ((gmlg.NUMBEROFCARS - gmlg.winCondition) -1) + " to go!";
                    break;
                case 6:
                    middleText.text = "Too slow Player " + gmlg.playerDeathNumber + "!";
                    break;
                case 7:
                    middleText.text = "We will develop a motorcycle for you next time Player " + gmlg.playerDeathNumber;
                    break;
                case 8:
                    middleText.text = "Give the controller to someone else Player " + gmlg.playerDeathNumber + "!";
                    break;
                case 9:
                    middleText.text ="Player " + gmlg.playerDeathNumber + " has been ANNIHILATED!";
                    break;
            }
            gmlg.playerDeathNumber = gmlg.NUMBEROFCARS;
            rand = 0;
        }
    }
}
