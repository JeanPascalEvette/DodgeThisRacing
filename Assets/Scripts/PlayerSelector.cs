using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerSelector : MonoBehaviour {


    public Dropdown dropdown;
    public Button playButton;

    void Awake()
    {
        playButton.onClick.AddListener(new UnityAction(SaveNumberToPlayerPrefs));
    }

    private void SaveNumberToPlayerPrefs()
    {
        int numberToSave;
        int selectedIndex = dropdown.value;
        if (int.TryParse(dropdown.options[selectedIndex].text, out numberToSave))
        {
            PlayerPrefs.SetInt("SavedNumber", numberToSave);
        }
        else
        {
            Debug.LogError("Chose option is no number: " + dropdown.options[selectedIndex].text);
        }
    }
}
    public void LoadLevel(string name)
    {

        SceneManager.LoadScene(name);

    }


    //create script for drop down menus that holds and returns number of players and computers 
    //max players + computers = 4.
    //public void onValueChanged()
    //{

    //}

}
