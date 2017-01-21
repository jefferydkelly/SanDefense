using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float timer = 0;
    float timeToDestroy = 10;
    float maxSpeed = 1;
    float damage = 10;
	Vector3 velocity;

    GameObject target;



    public void Initialize(GameObject target, float speed, float damage)
    {
        this.target = target;
        maxSpeed = speed;
        //rb.velocity = transform.forward * maxSpeed;
        this.damage = damage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
			if (!GameManager.Instance.IsPaused) {
				transform.position += maxSpeed * (target.transform.position - transform.position).normalized * Time.deltaTime;
	            
	            if (timer > timeToDestroy) Destroy(gameObject);
	            else timer += Time.deltaTime;
			}
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
