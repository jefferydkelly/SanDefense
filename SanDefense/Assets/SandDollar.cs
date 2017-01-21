using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollar : MonoBehaviour {

    private bool clicked = false;

    void OnMouseDown()
    {
        if (!clicked) {
           GetComponentInChildren<Animator>().SetBool("Collected", true);
           Destroy(gameObject, .75f);
           GameManager.Instance.funds(5);
           clicked = true;
        }
    }
}
