using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //timer to make sure bullets eventually die
    float timer = 0;
    float timeToDestroy = 10;

    //variables of bullets
    float maxSpeed = 1;
    float damage = 10;
    Vector3 velocity;

    //target of bullets
    GameObject target;
    Vector3 persistingTargetPos;

    //particle system and keeping track if exploded yet
    bool exploded = false;
    ParticleSystem explosion;

    /// <summary>
    /// Basically a constructor
    /// </summary>
    /// <param name="target">Target to attack</param>
    /// <param name="speed">Speed of projectile</param>
    /// <param name="damage">Damage of projectile</param>
    public void Initialize(GameObject target, float speed, float damage)
    {
        this.target = target;
        maxSpeed = speed;
        this.damage = damage;
    }

    //Initializing components
    public void Start()
    {
        explosion = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// For Updating the physics
    /// </summary>
    void FixedUpdate()
    {
        //Check if explosion hasn't happened and game is paused
        if (!GameManager.Instance.IsPaused && !exploded)
        {
            //check if target still exists
            if (target)
            {
                //moves bullet
                transform.position += maxSpeed * (target.transform.position - transform.position).normalized * Time.deltaTime;

                //update persisting target (in case target dies)
                persistingTargetPos = target.transform.position;
            }
            else
            {
                //move toward persisting location
                transform.position += maxSpeed * (persistingTargetPos - transform.position).normalized * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Updates every frame
    /// </summary>
    void Update()
    {
        //check if exploded
        if (!GameManager.Instance.IsPaused && !exploded)
        {
            //check if close enough to persisting target
            if (!target)
            {
                if ((persistingTargetPos - transform.position).sqrMagnitude < 1)
                    Explode();
            }

            //cleans up if somehow bullet doesn't reach anything
            if (timer > timeToDestroy) Destroy(gameObject);
            else timer += Time.deltaTime;
        }
    }

    //Called when bullet hits enemy
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Explode();
        }
    }

    /// <summary>
    /// Explodes the bullet
    /// </summary>
    void Explode()
    {
        //sets the bool, play the particles, disables renderer, then destroys the object
        exploded = true;
        explosion.Play();
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, explosion.main.duration);
    }
}
