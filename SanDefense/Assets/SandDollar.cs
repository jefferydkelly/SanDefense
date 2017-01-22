using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollar : MonoBehaviour
{

    public int value = 5;

    void OnMouseDown()
    {
        GetComponentInChildren<Animator>().SetBool("Collected", true);
        Destroy(gameObject, .75f);
		GameManager.Instance.Funds += value;
        Destroy(GetComponent<SphereCollider>());
        
    }
}
