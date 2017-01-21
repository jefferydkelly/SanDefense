using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    //The gamestate that the game is currently in
	GameStates gameState;

    //The different empty objects that contain different visuals
    public GameObject game;
    public GameObject difficulty;
    public GameObject mainMenu;
    public GameObject options;
    public GameObject credits;

    // Use this for initialization
    void Start () {
        //Start the game at the main menu
		GameState = GameStates.MainMenu;
	}
	
	// Update is called once per frame
	void Update () {

	}

	GameStates GameState {
		get {
			return gameState;
		}

		set {
			gameState = value;

			game.SetActive(gameState == GameStates.Game);
			if (gameState == GameStates.Game) {
				GameManager.Instance.StartGame ();
			}
			difficulty.SetActive(gameState == GameStates.Difficulty);

			mainMenu.SetActive(gameState == GameStates.MainMenu);
			options.SetActive(gameState == GameStates.Options);
			credits.SetActive(gameState == GameStates.Credits);
		}
	}
	public void SetGameState(string s) {
		GameState = (GameStates)System.Enum.Parse (typeof(GameStates), s);
	}

	public void Quit() {
		Application.Quit ();
	}
}

public enum GameStates {
	MainMenu,
	Difficulty,
	Game,
	Options,
	Credits
}
