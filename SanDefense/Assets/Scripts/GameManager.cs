using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager instance = null;
	bool paused = false;
	float curCastleHP = 0;
	[SerializeField]
	float maxCastleHP;
	int waveNumber;
	WaveState waveState;
	ImageBoxWithBackground msgBox;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
			curCastleHP = maxCastleHP;
			waveNumber = 0;
			msgBox = new ImageBoxWithBackground ("Message");
			StartSetup ();
		} else {
			Destroy (gameObject);
		}
	}

	void HideMessage() {
		msgBox.Enabled = false;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.P)) {
			paused = !paused;
		}
	}
	void StartWave() {
		waveState = WaveState.Wave;
		msgBox.Text = "Wave " + waveNumber + " Start";
		Invoke ("HideMessage", 5.0f);
		Grid.TheGrid.StartWave ();
		Invoke ("EndWave", 15 * (waveNumber + 1));
	}

	void EndWave() {
		waveState = WaveState.EndWave;
		msgBox.Text = "Wave Over";
		Invoke ("HideMessage", 5.0f);
		Grid.TheGrid.EndWave ();
		Invoke ("StartSetup", 30);
	}

	void StartSetup() {
		msgBox.Text = "Setup";
		Invoke ("HideMessage", 5.0f);
		waveState = WaveState.SetUp;
		waveNumber++;
		Invoke ("StartWave", 15);
	}
	/// <summary>
	/// Damages the castle.  If Castle HP drops below 0, the game's over.
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void DamageCastle(float dmg) {
		curCastleHP -= dmg;
	
		if (curCastleHP < 0) {
			Debug.Log ("Game over, man!  Game over");
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
