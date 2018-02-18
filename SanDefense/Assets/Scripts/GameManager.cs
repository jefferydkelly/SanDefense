using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private static GameManager instance = null;
	bool paused = false;
	int curCastleHP = 0;
    
	[SerializeField]
	int startMoney = 100;
	int moneyAmount;
    public int waveNumber;

    public Tide wave;
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
	Timer startWaveTimer;

	bool won = false;
    float gameScale = 1.0f;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
			msgBox = new ImageBoxWithBackground ("Message");
			msgBox.Enabled = false;
			hpText = castleHealthDisplay.GetComponentInChildren<Text>();
			waveText = waveDisplay.GetComponentInChildren<Text> ();
			waveDisplay.maxValue = maxWaves;
			moneyAmount = startMoney;
			moneyText.text = "Sand Dollars: " + moneyAmount.ToString();
         
			startWaveTimer = new Timer(15);
			startWaveTimer.OnComplete.AddListener(StartWave);
            EventManager.StartListening("WaveOver", EndWave);
            if (!Application.isMobilePlatform) {
                gameScale = 2.0f;
            }
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public IEnumerator StartGame() {
		if (!gameRunning) {
			GridManager.TheGrid.HideAllButtons ();
			gameRunning = true;
			paused = false;
			curCastleHP = maxCastleHP;
			waveNumber = 0;

            waveDisplay.value = waveNumber;

            castleHealthDisplay.maxValue = maxCastleHP;

            hpText.text = "HP " + maxCastleHP + " / " + maxCastleHP;
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
        if (Input.GetKeyDown (KeyCode.P) && UIManager.Instance.State == GameStates.Play) {
			paused = !paused;

			if (paused) {
                EventManager.Instance.TriggerEvent("Pause");
            } else {
                EventManager.Instance.TriggerEvent("Unpause");
            }
		}

		if (!paused) {
            TimerManager.Instance.Update(Time.deltaTime);
		}
	}
	public void StartWave() {
		if (waveState == WaveState.SetUp) {
			TimerManager.Instance.RemoveTimer (startWaveTimer);
			waveState = WaveState.Wave;
			msgBox.Text = "Wave " + waveNumber + " Start";
			Invoke ("HideMessage", 2.0f);
			GridManager.TheGrid.StartWave ();
			//TimerManager.Instance.AddTimer (new Timer (endWaveDelegate, 15 * (waveNumber + 1)));
		}
	}

	void EndWave() {
		GridManager.TheGrid.HideAllButtons ();
		waveState = WaveState.EndWave;
		msgBox.Text = "Wave Over";
		Invoke ("HideMessage", 2.0f);
		GridManager.TheGrid.ClearRocks ();
		GridManager.TheGrid.ScatterRocks (Random.Range (1, Mathf.Min (waveNumber + 3, 6)) * 2);
		GridManager.TheGrid.EndWave ();
		wave.RollTide (waveNumber + 1);
        Timer setupTimer = new Timer(2);
        setupTimer.OnTick.AddListener(StartSetup);
        setupTimer.Start();
	}

	void StartSetup() {
		waveState = WaveState.SetUp;
		GridManager.TheGrid.SetClickState ("None");
		msgBox.Text = "Setup";

		castleHealthDisplay.value = maxCastleHP;
		Invoke ("HideMessage", 2.0f);

		waveNumber++;

        if (waveNumber < maxWaves) {
			waveText.text = "Wave " + waveNumber + " / " + maxWaves;
			waveDisplay.value = waveNumber;
			
            startWaveTimer.Stop();
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
		hpText.text = "HP" + curCastleHP + " / " + maxCastleHP;
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
		Funds = startMoney;
		GridManager.TheGrid.Reset ();
        EventManager.Instance.TriggerEvent("Restart");
		//UIManager.Instance.SetGameState ("Game");
	}

	public bool WonGame {
		get {
			return won;
		}
	}

	public int Funds {
		get {
			return moneyAmount;
		}

		set {
			moneyAmount = value;
			moneyText.text = "Sand Dollars: " + moneyAmount.ToString();
		}
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

    public float Scale {
        get {
            return gameScale;
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