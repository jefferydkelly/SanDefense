using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTanat : MonoBehaviour {

    public GameObject EnemyPrefab;

	// Use this for initialization
	void Start () {
        EnemyManager.Instance.Enemies.Add(Instantiate(EnemyPrefab));
        EnemyManager.Instance.Enemies.Add(Instantiate(EnemyPrefab));

    }

    // Update is called once per frame
    void Update () {
		
	}
}
