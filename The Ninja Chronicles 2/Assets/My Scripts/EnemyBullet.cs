using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;

    public int destroyTimer = 5;

    // Start is called before the first frame update
    void Start()
    { 
        Destroy(gameObject, destroyTimer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player p = other.GetComponent<Player>();

            if (p != null)
                p.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
