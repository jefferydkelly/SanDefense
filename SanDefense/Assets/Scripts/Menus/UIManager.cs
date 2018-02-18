using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	private static UIManager instance;
    //The gamestate that the game is currently in
    GameStates state;

    //The different empty objects that contain different visuals
    public GameObject game;
    public GameObject difficulty;
    public GameObject mainMenu;
    public GameObject credits;
    public GameObject instructions;
    public GameObject pause;

    // Use this for initialization
    void Start () {
		if (instance == null) {
			instance = this;
            //Start the game at the main menu
            ShowMainMenu();
            EventManager.StartListening("Pause", Pause);
            EventManager.StartListening("Start Game", StartGame);
		} else {
			Destroy (gameObject);
		}
	}

    void Pause() {
        EventManager.StopListening("Pause", Pause);
        state = GameStates.Pause;
        pause.SetActive(true);
        EventManager.StartListening("Unpause", Unpause);
    }

    void Unpause() {
        EventManager.StopListening("Unpause", Unpause);
        State = GameStates.Play;
        StartCoroutine(GameManager.Instance.StartGame());
        EventManager.StartListening("Pause", Pause);
    }

    void ShowMainMenu() {
        EventManager.StopListening("Show Main Menu", ShowMainMenu);
        State = GameStates.MainMenu;
        EventManager.StartListening("Show Credits", ShowCredits);
        EventManager.StartListening("Show Instructions", ShowInstructions);

    }

    void ShowCredits() {
        EventManager.StopListening("Show Credits", ShowCredits);
        State = GameStates.Credits;
        EventManager.StartListening("Show Main Menu", ShowMainMenu);

    }

    void ShowDifficulty() {
        EventManager.StopListening("Show Difficulty", ShowDifficulty);
        State = GameStates.Difficulty;
        EventManager.StartListening("Show Main Menu", ShowMainMenu);
    }

    void ShowInstructions() {
        EventManager.StopListening("Show Instructions", ShowInstructions);
        State = GameStates.Instructions;
        EventManager.StartListening("Show Main Menu", ShowMainMenu);
    }

	public GameStates State
    {
        get
        {
            return state;
        }


        private set
        {
            state = value;

            game.SetActive(state == GameStates.Play);
            difficulty.SetActive(state == GameStates.Difficulty);

            mainMenu.SetActive(state == GameStates.MainMenu);
            credits.SetActive(state == GameStates.Credits);
            instructions.SetActive(state == GameStates.Instructions);

            pause.SetActive(state == GameStates.Pause);
        }
    }
    public void SetGameState(GameStates s) {
        State = s;//(GameStates)System.Enum.Parse (typeof(GameStates), s);
	}

   void StartGame() {
        State = GameStates.Play;
        StartCoroutine(GameManager.Instance.StartGame());
    }

	public void Quit() {
		Application.Quit ();
	}

	public static UIManager Instance {
		get {
			return instance;
		}
	}
}

public enum GameStates {
	MainMenu,
	Difficulty,
    Play,
	Credits,
    Instructions,
    Pause
}
