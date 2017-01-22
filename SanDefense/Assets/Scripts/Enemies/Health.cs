using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    //The maximum health of the enemy
    //The amount of health the enemy currently has
    public float maxHealth;
    private float currentHealth;

    //The size of the health bar when full
    //The health bar
    private float originalScale;
    private Transform healthBar;

    public GameObject[] Drops;
    private ParticleSystem particleSystem;

    public GameObject[] limbs;

    public float CurHealth
    {
        get { return currentHealth; }
    }

	// Use this for initialization
	void Start () {

        //Set the current health to the maximum health
		maxHealth += 25 * GameManager.Instance.waveNumber;
		currentHealth = maxHealth;

        //Get the health bar
        healthBar = gameObject.GetComponentsInChildren<Transform>()[1];

        //Set the maximum size for the health bar
        originalScale = healthBar.localScale.x;

        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {

        //Get the size of the health bar
        //Calculate the new width of the health bar
        //Set the new size of the health bar
        Vector3 newScale = healthBar.localScale;
        newScale.x = currentHealth / maxHealth * originalScale;
        healthBar.localScale = newScale;

        
	}

    //Deal damage to the enemy
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        particleSystem.Play();

		//Test if the current health of the enemy is less than 0
		//Destroy the enemy
		if (currentHealth <= 0)
		{
            Instantiate(Drops[Random.Range(0,Drops.Length)], transform.position + Vector3.up * 1, Quaternion.identity);
			EnemyManager.Instance.Enemies.Remove (gameObject);
            //Destroy(GetComponent<Movement>());
            //Destroy(GetComponentInChildren<Animator>());
            //foreach(GameObject limb in limbs)
            //{
            //    limb.AddComponent<BoxCollider>();
            //    limb.AddComponent<Rigidbody>();
            //    limb.GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 2);
            //}
			Destroy(gameObject);
            //Destroy(this);
		}
    }

    //Heal the enemy
    void Heal(float amount)
    {
        currentHealth += amount;
    }
}
