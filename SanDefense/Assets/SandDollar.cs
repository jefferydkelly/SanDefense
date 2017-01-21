using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollar : MonoBehaviour
{
    void OnMouseDown()
    {
        GetComponentInChildren<Animator>().SetBool("Collected", true);
        Destroy(gameObject, .75f);
    }
}
