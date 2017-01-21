using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void buttonClick(string name) {
        
        switch(name)
        {
            case "mainMenu":
                GetComponentInParent<GameState>().gameState = "mainMenu";
                break;
        }
    }
}
