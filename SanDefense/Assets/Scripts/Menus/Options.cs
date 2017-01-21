using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void buttonClick(string name) {

        //Test if the main menu button is clicked
        //Set the game state back to the main menu
        if (name == "mainMenu")
        {
            GetComponentInParent<GameState>().gameState = "mainMenu";
            return;
        }
    }
}
