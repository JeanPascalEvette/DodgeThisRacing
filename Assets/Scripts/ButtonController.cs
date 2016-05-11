using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

    private bool overlay = false;
    private PlayerSelector playerSelector;

    void Start()
    {
        playerSelector = transform.GetComponentInParent<PlayerSelector>();
        if (playerSelector == null)
        {
            Debug.LogError("Player Selector not found");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay = false;
        }
    }

    void Update()
    {
        if (overlay)
        {
            if (Input.GetButtonDown("ButtonAArrows"))
            {
                playerSelector.PanelManager();
            }                
        }
    }

    public void SetOverlay(bool state)
    {
        overlay = state;
    }

}
