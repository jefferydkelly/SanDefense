using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollar : ClickableObject
{
    [SerializeField]
    int value = 5;

    public override void OnClick()
    {
        GetComponentInChildren<Animator>().SetBool("Collected", true);
        Destroy(gameObject, .75f);
		GameManager.Instance.Funds += value;
        Destroy(GetComponent<SphereCollider>());
        
    }
}
