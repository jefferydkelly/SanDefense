using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private static GameManager instance = null;
	bool paused = false;
	int curCastleHP = 0;
    public int moneyAmount = 100;
    public int waveNumber;

    public Wave wave;
    [SerializeField]
	int maxCastleHP;

	[SerializeField]
	int maxWaves = 10;

	WaveState waveState;
	ImageBoxWithBackground msgBox;
	public Slider castleHealthDisplay;
	Text hpText;
	public Slider waveDisplay;
	Text waveText;
	public Text moneyText;
	bool gameRunning = false;

	Coroutine currentCoroutine;
	WaitDelegate startWaveDelegate;
	WaitDelegate endWaveDelegate;
	WaitDelegate startSetupDelegate;

	bool won = false;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
			msgBox = new ImageBoxWithBackground ("Message");
			msgBox.Enabled = false;
			hpText = castleHealthDisplay.GetComponentInChildren<Text>();
			waveText = waveDisplay.GetComponentInChildren<Text> ();
			moneyText.text = "\t" + moneyAmount.ToString();
            startWaveDelegate = () => {
				StartWave ();
			};
			endWaveDelegate = () => {
				StartCoroutine(EndWave ());
			};
			startSetupDelegate = () => {
				StartSetup();
			};
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public IEnumerator StartGame() {
		if (!gameRunning) {
			Grid.TheGrid.HideAllButtons ();
			gameRunning = true;
			paused = false;
			curCastleHP = maxCastleHP;
			waveNumber = 0;

            waveDisplay.value = waveNumber;

            castleHealthDisplay.maxValue = maxCastleHP;


			hpText.text = maxCastleHP + " / " + maxCastleHP;
			yield return StartCoroutine(wave.RollTide (waveNumber+1));//randomWaveSize(waveNumber + 1);

			StartSetup ();
		} else if (paused) {
			paused = false;
		}
	}
	void HideMessage() {
		msgBox.Enabled = false;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.P)) {
			paused = !paused;

			if (paused) {
				UIManager.Instance.SetGameState ("Pause");
			}
		}
	}
	void StartWave() {

		waveState = WaveState.Wave;
		msgBox.Text = "Wave " + waveNumber + " Start";
		Invoke ("HideMessage", 2.0f);
		Grid.TheGrid.StartWave ();
		currentCoroutine = StartCoroutine (gameObject.RunAfter(endWaveDelegate, 30 * (waveNumber + 1)));
	}

	IEnumerator EndWave() {
		Grid.TheGrid.HideAllButtons ();
		waveState = WaveState.EndWave;
		msgBox.Text = "Wave Over";
		Invoke ("HideMessage", 2.0f);

		Grid.TheGrid.EndWave ();

		yield return StartCoroutine(wave.RollTide (waveNumber + 1));

        currentCoroutine = StartCoroutine (gameObject.RunAfter(startSetupDelegate, 2));
	}

	void StartSetup() {
		Grid.TheGrid.SetClickState ("None");
		msgBox.Text = "Setup";

		castleHealthDisplay.value = maxCastleHP;
		Invoke ("HideMessage", 2.0f);
		waveState = WaveState.SetUp;
		waveNumber++;

        if (waveNumber < maxWaves) {
			waveText.text = "\t" + waveNumber + " / " + maxWaves;
			waveDisplay.value = waveNumber;
			currentCoroutine = StartCoroutine (gameObject.RunAfter (startWaveDelegate, 15));
		} else {
			won = true;
			SceneManager.LoadScene ("GameOver");
		}

	}
	/// <summary>
	/// Damages the castle.  If Castle HP drops below 0, the game's over.
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void DamageCastle(int dmg) {
		curCastleHP -= dmg;
		castleHealthDisplay.value = curCastleHP;
		hpText.text = curCastleHP + " / " + maxCastleHP;
		if (curCastleHP < 0) {
			SceneManager.LoadScene ("GameOver");
		}
	}

	/// <summary>
	/// Gets the instance of Game Manager.
	/// </summary>
	/// <value>The instance.</value>
	public static GameManager Instance {
		get {
			return instance;
		}
	}

	public bool IsPaused {
		get {
			return paused;
		}
	}

	public void RestartGame() {
		gameRunning = false;
		Grid.TheGrid.Clear();
		StopCoroutine (currentCoroutine);
		UIManager.Instance.SetGameState ("Game");
	}

	public bool WonGame {
		get {
			return won;
		}
	}
    public void funds(int price)
    {
        moneyAmount += price;
		moneyText.text = "\t" + moneyAmount.ToString();
    }

	public int CurWave {
		get {
			return waveNumber;
		}
	}

	public WaveState WaveState {
		get {
			return waveState;
		}
	}
}

public enum WaveState {
	SetUp,
	Wave,
	EndWave
}

public struct ImageBoxWithBackground {
	Image img;
	Text txt;
	public ImageBoxWithBackground(string name) {
		img = GameObject.Find (name).GetComponent<Image> ();
		txt = img.GetComponentInChildren<Text> ();
	}

	public bool Enabled {
		get {
			return img.enabled;
		}

		set {
			img.enabled = value;
			txt.enabled = value;
		}
	}

	public string Text {
		get {
			return txt.text;
		}

		set {
			txt.text = value;
			Enabled = true;
		}
	}
}