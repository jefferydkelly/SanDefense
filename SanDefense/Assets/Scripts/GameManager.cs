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
	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
			curCastleHP = maxCastleHP;
			waveNumber = 1;
		} else {
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Damages the castle.  If Castle HP drops below 0, the game's over.
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void DamageCastle(float dmg) {
		curCastleHP -= dmg;
		Debug.Log (curCastleHP);
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
