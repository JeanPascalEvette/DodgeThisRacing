using UnityEngine;
using System.Collections;

//This Script is assigned to each coin object to detect if its cursor is close to it or not. If the cursor is close it can pick up the coin, otherwise it can't
public class ButtonCollider : MonoBehaviour {

    public GameObject player;          //Instance of the Cursor/Player that holds this coin (Assigned by the Inspector)
    public bool is_player_near = true; //Bool Variable Telling if the cursor is Close to the coin or not by default it starts close and attached to it


    //Function to detect entering the collision of the coin with a cursor
    void OnTriggerEnter2D(Collider2D trigger)
    {
        //If the cursor is the correct one (Player1 to Coin1 , Player2 to Coin2, etc...) The boolian is set to true (the Cursor is close enough to the coin to pick it up)
        if (trigger == player.GetComponent<Collider2D>()) {is_player_near = true;}

        //Debug stuff(Delete later)
        print("Enter Button" + is_player_near);
    }



    //Function to detect exiting the collision of the coin with a cursor
    void OnTriggerExit2D(Collider2D trigger)
    {
        //If the cursor is the correct one (Player1 to Coin1 , Player2 to Coin2, etc...) The boolian is set to false (The cursor is too far away from the coin to pick it up)
        if (trigger == player.GetComponent<Collider2D>()) {is_player_near = false;}

        //Debug stuff(Delete later)
        print("Exit Button"+ is_player_near);
    }
}
