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

    [Range(0, 3)]
    public float attackCooldown = 1;

    [Range(0.5f,10)]
    public float bulletSpeed = 2;

    [Range(3, 20)]
    public float radius = 5;
    private float radiusSqr = 25;

    private BoxCollider boxCollider;
    private Transform head;
    private GameObject[] enemies;
    private GameObject target;
    private float timer = 0;
    private Vector3 targetForward;


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
        DetermineTarget();

        targetForward = head.forward;

        if (target)
        {
            DetermineDirection();
            Shoot();
        }


        head.forward = Vector3.Lerp(head.forward, targetForward, .5f);
        timer += Time.deltaTime;
    }

    void DetermineTarget()
    {
        switch (attackStyle)
        {
            case AttackStyle.AttackLowest:
                target = null;
                break;
            case AttackStyle.AttackFurthest:
                target = null;
                float furthestDist = 0;

                //loops through all enemies
                foreach (GameObject enemy in enemies)
                {
                    //check if enemy is dead
                    if (enemy)
                    {
                        Vector3 dist = enemy.transform.position - transform.position;

                        dist.y = 0;

                        //print(dist.magnitude + " " + radius);
                        //print(dist.magnitude > furthestDist);

                        //checks if in radius and if further than current max
                        if (dist.sqrMagnitude < radiusSqr &&
                            dist.magnitude < radius &&
                            dist.sqrMagnitude > Mathf.Pow(furthestDist, 2) &&
                            dist.magnitude > furthestDist)
                        {
                            //print("Targeting: " + enemy.gameObject.name + " " + furthestDist);
                            target = enemy;
                            furthestDist = dist.magnitude;
                        }
                    }
                }
                break;
            case AttackStyle.AttackFirstEnemy:
                //breaks 
                if (target) break;
                foreach (GameObject enemy in enemies)
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
    }

    /// <summary>
    /// Determines direction based on where the enemy is
    /// </summary>
    void DetermineDirection()
    {
        targetForward = (target.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// Checks the cooldown and shoots at the enemy
    /// </summary>
    void Shoot()
    {
        if (timer > attackCooldown)
        {
            //print("Shot " + target.name);
            Bullet bul = (Instantiate(bullet, transform.position, head.transform.rotation, transform)).GetComponent<Bullet>();
            bul.Initialize(target, bulletSpeed);
            timer = 0;
        }
    }

    //for visualizing radius
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
