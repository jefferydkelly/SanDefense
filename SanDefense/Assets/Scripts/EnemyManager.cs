using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (instance)
                return instance;
            else
            {
                print("Fuck there's no Enemy ManageR");
                return instance;
            }
        }
    }

    private GameObject[] enemies;
    public GameObject[] Enemies
    {
        get { return enemies; }
    }

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void OnDestroy()
    {
        instance = null;
    }

}
