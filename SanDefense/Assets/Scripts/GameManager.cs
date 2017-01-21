using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager instance = null;
	bool paused = false;
	float curCastleHP = 0;
	[SerializeField]
	float maxCastleHP;
	int waveNumber;
	WaveState waveState;
	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
			curCastleHP = maxCastleHP;
			waveNumber = 0;
			StartSetup ();
		} else {
			Destroy (gameObject);
		}
	}

	void StartWave() {
		Debug.Log ("The wave is started");
		waveState = WaveState.Wave;
		Grid.TheGrid.StartWave ();
		Invoke ("EndWave", 15 * (waveNumber + 1));
	}

	void EndWave() {
		Debug.Log ("The wave has ended");
		waveState = WaveState.EndWave;
		Grid.TheGrid.EndWave ();
		Invoke ("StartSetup", 30);
	}

	void StartSetup() {
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
}

public enum WaveState {
	SetUp,
	Wave,
	EndWave
}
