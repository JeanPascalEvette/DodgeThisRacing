using UnityEngine;
using System.Collections;

public class ButtonCollider : MonoBehaviour {

    public GameObject player;
    public bool is_player_near = true;

	//// Use this for initialization
	//void Start () {
	
	//}
	
	//// Update is called once per frame
	//void Update () {

        
	//}


    void OnTriggerEnter2D(Collider2D trigger)
    {

        if (trigger == player.GetComponent<Collider2D>()) {

            is_player_near = true;
        }

        print("Enter Button" + is_player_near);
    }

    
    void OnTriggerExit2D(Collider2D trigger)
    {

        if (trigger == player.GetComponent<Collider2D>())
        {

            is_player_near = false;
        }

        print("Exit Button"+ is_player_near);
    }
}
