using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!Application.isMobilePlatform)
        {
            transform.position = transform.position.SetY(Screen.height - 175);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
