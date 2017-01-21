using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class Tower : MonoBehaviour
{
    public enum AttackStyle
    {
        AttackFirstEnemy,
        AttackFurthest,
        AttackLowest,
    };

    public GameObject bullet; //prefab of bullet to 
    public GameObject turretHead; //this turns and shoot, if none use the game object this is attached to to turn
    public AttackStyle attackStyle = AttackStyle.AttackLowest; //ai attack style

    [Range(3,20)]
    public float radius = 5;
    private float radiusSqr = 25;

    private BoxCollider boxCollider;
    private Transform head;
    private GameObject[] enemies;
    private GameObject target;


    // Use this for initialization
    void Start()
    {
        //initializing variables
        boxCollider = GetComponent<BoxCollider>();
        if (turretHead)
            head = turretHead.transform;
        else head = transform;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        radiusSqr = Mathf.Pow(radius, 2);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        switch (attackStyle)
        {
            case AttackStyle.AttackLowest:
                target = null;
                break;
            case AttackStyle.AttackFurthest:
                target = null;

                //loops through all enemies
                foreach (GameObject enemy in enemies)
                {
                    //check if enemy is dead
                    if (enemy)
                    {
                        Vector3 dist = enemy.transform.position - transform.position;
                        float furthestDist = float.MinValue;

                        //checks if in radius and if further than current max
                        if (dist.sqrMagnitude < radiusSqr && 
                            dist.magnitude < radius && 
                            dist.sqrMagnitude > Mathf.Pow(furthestDist,2) &&
                            dist.magnitude > furthestDist)
                        {
                            target = enemy;
                            furthestDist = dist.magnitude;
                        }
                    }
                }
                break;
            case AttackStyle.AttackFirstEnemy:
                //breaks 
                if (target) break;
                foreach(GameObject enemy in enemies)
                {
                    //check if enemy is dead
                    if (enemy)
                    {
                        Vector3 dist = enemy.transform.position - transform.position;

                        if (dist.sqrMagnitude < radiusSqr && dist.magnitude < radius)
                        {

                        }
                        else continue;
                    }
                }
                break;
        }

        if (target)
        {
            head.forward = (target.transform.position - transform.position).normalized;
            Instantiate(bullet, transform.position, head.transform.rotation, transform);
        }
    }

    //for visualizing radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
