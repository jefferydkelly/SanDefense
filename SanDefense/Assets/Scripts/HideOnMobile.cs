using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnMobile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Application.isMobilePlatform) {
            gameObject.SetActive(false);
        }
	}
}
