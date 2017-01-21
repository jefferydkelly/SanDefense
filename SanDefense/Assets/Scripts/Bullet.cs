using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float timer = 0;
    float timeToDestroy = 10;

    GameObject target;

    public void Initialize(GameObject target)
    {
        this.target = target;
    }



    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 pos = transform.position;
            pos += 0.2f * (target.transform.position - pos).normalized;
            transform.position = pos;

            if (timer > timeToDestroy) Destroy(gameObject);
            else timer += Time.deltaTime;
        }
        else
        {
            print("No target for " + gameObject.name + ". Destroying self now");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
