using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Difficulties : MonoBehaviour {

    //The button that is being hovered over
    //The button that is selected
    public string hover;
    public string selected;

    public UnityEngine.UI.Text  difficultyInfo;

	// Use this for initialization
	void Start () {
        //Set that no buttons are selected to begin with
        selected = null;
	}
	
	// Update is called once per frame
	void Update () {

        //Test if there is a button selected
        //Start the game
        //Reset the selected to be null
        if (selected != null)
        {
            GetComponentInParent<GameState>().gameState = "game";
            selected = null;
        }

        //How many waves of creatures there will be
        //The multiplier for the amount of money you get
        //The score multiplier
        //How many creature waves must happen before a water wave happens
        //The amount of towers that can be destroyed in a water wave
        int creatureWaves = 0;
        int currencyModifier = 0;
        float scoreBonus = 0;
        int waveFreq = 0;
        int waveDest = 0;

        //Test which difficulty is being hovered over
        //Display information about the difficulty level
        switch(hover)
        {
            case "Easy":
                creatureWaves = 10;
                currencyModifier = 150;
                scoreBonus = .75f;
                waveFreq = 3;
                waveDest = 1;
                break;
            case "Medium":
                creatureWaves = 15;
                currencyModifier = 100;
                scoreBonus = 1;
                waveFreq = 2;
                waveDest = 2;
                break;
            case "Hard":
                creatureWaves = 25;
                currencyModifier = 75;
                scoreBonus = 1.5f;
                waveFreq = 1;
                waveDest = 2;
                break;
        }

        //Display the information about the difficulty off to the left
        difficultyInfo.text =
            "Game Difficulty: " + hover + "\n\n" +
            "Currency Bonus: " + currencyModifier + "\n\n" +
            "Amount of Creature Waves: " + creatureWaves + "\n\n" +
            "Score Multiplier: " + scoreBonus + "\n\n" +
            "One Wave every " + waveFreq + " creature waves\n\n" +
            "Up to " + waveDest + " towers will be destroyed by the wave";
	}

    public void buttonClick(string level)
    {

        //Test if the button clicked is the main menu button
        //Return to the main menu
        if (level == "mainMenu")
        {
            GetComponentInParent<GameState>().gameState = "mainMenu";
            return;
        }

        //Set the name for which button that was clicked
        selected = level;
    }

    public void buttonEnter(string level)
    {
        //Set the name for which button is being hovered over
        hover = level;
    }

    public void buttonExit()
    {
        //Set the name for which button is being hovered over
        hover = "";
    }
}
