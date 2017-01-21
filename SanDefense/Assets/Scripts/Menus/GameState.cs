using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    //The gamestate that the game is currently in
    public string gameState;

    //The different empty objects that contain different visuals
    public GameObject game;
    public GameObject difficulty;
    public GameObject mainMenu;
    public GameObject options;
    public GameObject credits;

    // Use this for initialization
    void Start () {
        //Start the game at the main menu
        gameState = "mainMenu";
	}
	
	// Update is called once per frame
	void Update () {

        //Test the game state
        //Activate the current empty object that should display
        //Deactivate all the other empty objects
        if (gameState == "game")
            game.SetActive(true);
        else
            game.SetActive(false);

        if (gameState == "difficulty")
            difficulty.SetActive(true);
        else
            difficulty.SetActive(false);

        if (gameState == "mainMenu")
            mainMenu.SetActive(true);
        else
            mainMenu.SetActive(false);

        if (gameState == "options")
            options.SetActive(true);
        else
            options.SetActive(false);

        if (gameState == "credits")
            credits.SetActive(true);
        else
            credits.SetActive(false);
	}
}
