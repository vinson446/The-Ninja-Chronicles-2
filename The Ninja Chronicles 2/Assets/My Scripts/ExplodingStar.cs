using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingStar : MonoBehaviour
{
    public float rotationSpeed = 180f;

    public GameObject explosion;
    bool hasExploded = false;
    public int damage = 20;
    public float radius = 6f;

    BoxCollider coll;
    MeshRenderer mr;

    public float destroyThis = 5f;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        transform.rotation = player.transform.rotation;

        damage = player.power;
        radius = 3 + player.level / 2;

        coll = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();

        Invoke("DestroyGameObject", destroyThis);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity);
            explo.transform.localScale = new Vector3(radius, radius, radius);
            Explosion();
            hasExploded = true;

            mr.enabled = false;
            coll.enabled = false;
        }
    }

    void Explosion()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius);
  
        foreach (Collider e in enemies)
        {
            BaseEnemy enemy = e.GetComponentInParent<BaseEnemy>();

            if (enemy != null)
                enemy.TakeDamage(damage);

            BaseShooter enemy2 = e.GetComponentInParent<BaseShooter>();

            if (enemy2 != null)
                enemy2.TakeDamage(damage);
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
