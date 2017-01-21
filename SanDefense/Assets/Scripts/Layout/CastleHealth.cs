using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CastleHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Slider>().maxValue = GetComponentInParent<GameInfo>().maxCastleHealth;
        GetComponent<Slider>().value = GetComponentInParent<GameInfo>().maxCastleHealth;
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Slider>().value = GetComponentInParent<GameInfo>().currentHealth;
    }
}
