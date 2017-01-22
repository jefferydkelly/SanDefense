using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollar : MonoBehaviour
{

    public int value = 5;

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            GetComponentInChildren<Animator>().SetBool("Collected", true);
            Destroy(gameObject, .75f);
            GameManager.Instance.funds(value);
            Destroy(GetComponent<SphereCollider>());
        }
    }
}
