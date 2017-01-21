using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameInfo : MonoBehaviour {

    public int maxWaves;
    public int currentWave;
    public int currentMoney;
    public float maxCastleHealth;
    public float currentHealth;

    private int timer = 0;

    public UnityEngine.UI.Text healthDisplay;
    public UnityEngine.UI.Text waveDisplay;
    public UnityEngine.UI.Text moneyDisplay;

	// Use this for initialization
	void Start () {
        currentHealth = maxCastleHealth;
	}
	
	// Update is called once per frame
	void Update () {

        if(timer > 100)
        {
            timer = 0;
            takeDamage(1);
        }

        timer++;

        healthDisplay.text = currentHealth + " / " + maxCastleHealth;
        waveDisplay.text = "\t" + currentWave + " / " + maxWaves;
        moneyDisplay.text = "\t" + currentMoney; 
	}

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
    }
}
