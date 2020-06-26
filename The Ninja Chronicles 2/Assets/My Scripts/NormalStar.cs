using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStar : MonoBehaviour
{
    public float rotationSpeed = 180f;

    public int damage = 10;

    Rigidbody rb;
    BoxCollider coll;

    public float destroyTimer = 5f;

    GameObject playerBody;
    Player player;

    Vector3 tmpScale;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
        transform.rotation = playerBody.transform.rotation;

        player = FindObjectOfType<Player>();

        if (this.tag == "DamnationShuriken")
            damage = player.power * 3;
        else
            damage = player.power;

        tmpScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();

        Destroy(gameObject, destroyTimer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Enemy2" && collision.gameObject.tag != "Line" && collision.gameObject.tag != "Player" && collision.gameObject.tag != "NormalShuriken" && collision.gameObject.tag != "ExplosionStar" && collision.gameObject.tag != "DamnationShuriken")
        {
            rotationSpeed = 0;
            rb.isKinematic = true;

            var emptyObject = new GameObject();
            emptyObject.AddComponent<DestroyGameObj>();
            emptyObject.transform.parent = collision.transform;
            transform.parent = emptyObject.transform;

            coll.enabled = false;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            BaseEnemy e = collision.gameObject.GetComponentInParent<BaseEnemy>();
            e.TakeDamage(damage);

            rotationSpeed = 0;
            rb.isKinematic = true;
            transform.parent = collision.transform.GetChild(0);

            coll.enabled = false;

            damage = 0;
        }

        if (collision.gameObject.tag == "Enemy2")
        {
            BaseShooter s = collision.gameObject.GetComponentInParent<BaseShooter>();
            s.TakeDamage(damage);

            rotationSpeed = 0;
            rb.isKinematic = true;
            transform.parent = collision.transform.GetChild(0);

            coll.enabled = false;

            damage = 0;
        }
    }
}
