using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    float timer = 0;
    float timeToDestroy = 10;
    float maxSpeed = 1;
    float damage = 10;

    GameObject target;

    Rigidbody rb;



    public void Initialize(GameObject target, float speed, float damage)
    {
        this.target = target;
        maxSpeed = speed;
        rb.velocity = transform.forward * maxSpeed;
        this.damage = damage;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
			//JD: I tested this just to see what it would look like if it went directly at the enemy instead of looping around like yours.
			rb.velocity = maxSpeed * (target.transform.position - transform.position).normalized;
            
			//rb.AddForce(maxSpeed * (target.transform.position - transform.position).normalized - rb.velocity);

            if (timer > timeToDestroy) Destroy(gameObject);
            else timer += Time.deltaTime;
        }
        else
        {
            print("No target for " + gameObject.name + ". Destroying self now");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
