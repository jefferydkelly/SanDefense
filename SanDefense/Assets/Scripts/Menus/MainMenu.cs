using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void buttonClick(string name)
    {
        //Test the name of the button that is pressed
        //Change the game state relative to the button that is pressed
        switch (name)
        {
            case "play":
                GetComponentInParent<GameState>().gameState = "difficulty";
                break;
            case "options":
                GetComponentInParent<GameState>().gameState = "options";
                break;
            case "credits":
                GetComponentInParent<GameState>().gameState = "credits";
                break;
            case "exit":
                Application.Quit();
                break;
        }
    }
}
