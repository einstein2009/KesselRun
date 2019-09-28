using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed;

    void Update()
    {
        transform.position += transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth hp = other.gameObject.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                hp.Die();
            } else
            {
                Destroy(other.gameObject);
            }
            Destroy(this);
        }
    }

}
