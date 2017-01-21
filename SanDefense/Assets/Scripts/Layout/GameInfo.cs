using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameInfo : MonoBehaviour {

    //The amount of creature waves to fight
    //The current creature wave
    //The current amount of money you have
    //The maximum health that the castle can have
    //The current health that the cast has
    public int maxWaves;
    public int currentWave;
    public int currentMoney;
    public float maxCastleHealth;
    public float currentHealth;

    //The slider indicator for the health
    //The slider indicator for the creature wave
    public Slider healthSlider;
    public Slider waveSlider;

    //Where the health is displayed
    //Where the wave is displayed
    //Where the money is displayed
    public UnityEngine.UI.Text healthDisplay;
    public UnityEngine.UI.Text waveDisplay;
    public UnityEngine.UI.Text moneyDisplay;

	// Use this for initialization
	void Start () {

        //Set the max amount of creature waves to fight
        //Set the current wave that is being fought
        waveSlider.maxValue = maxWaves;
        waveSlider.value = currentWave;

    }

	// Update is called once per frame
	void Update () {
        //healthDisplay.text = currentHealth + " / " + maxCastleHealth;
        waveDisplay.text = "\t" + currentWave + " / " + maxWaves;
        moneyDisplay.text = "\t" + currentMoney;

        //healthSlider.value = currentHealth;
        waveSlider.value = currentWave;
    }

    public void takeDamage(int amount)
    {
        //Take x amount of damage from the castle health
        currentHealth -= amount;
    }

    public void nextWave()
    {
        //Set the next wave
        currentWave++;
    }
}
