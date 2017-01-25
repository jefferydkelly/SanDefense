using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager
{
	[SerializeField]
	Slider enemiesKilledSlider;
	Text ekText;
    private static EnemyManager instance;
	public EnemyManager() {
		enemies = new List<GameObject> ();
		enemiesKilledSlider = GameObject.Find ("EnemySlider").GetComponent<Slider> ();
		ekText = enemiesKilledSlider.GetComponentInChildren<Text> ();
	}
    public static EnemyManager Instance
    {
        get
        {
			if (instance == null)
				instance = new EnemyManager ();
            
			return instance;
        }
    }

	private List<GameObject> enemies;
	public List<GameObject> Enemies
    {
        get { return enemies; }
    }

	public int NumberOfEnemiesAlive {
		get {
			return enemies.Count;
		}
	}

	public void StartWave(int numEnemies) {
		enemiesKilledSlider.maxValue = numEnemies;
		enemiesKilledSlider.value = 0;
		ekText.text = "0 /" + enemiesKilledSlider.maxValue;
	}
	public void RemoveEnemy(GameObject go) {
		if (enemies.Remove (go)) {
			enemiesKilledSlider.value++;
			ekText.text = enemiesKilledSlider.value + " / " + enemiesKilledSlider.maxValue;
		}
	}

	public bool AllEnemiesKilled {
		get {
			return enemiesKilledSlider.value == enemiesKilledSlider.maxValue;
		}
	}
}
