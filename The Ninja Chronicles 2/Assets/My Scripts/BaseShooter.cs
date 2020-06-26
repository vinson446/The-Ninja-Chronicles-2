using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseShooter : MonoBehaviour
{
    [Header("Enemy Systems")]
    [Space(5)]
    public float health = 60f;
    public int expAmt = 10;
    bool isDead = false;

    public int range = 10;
    //public int aggroRange;
    float distance;

    public float attackSpeed = 1f;
    public int damage = 10;
    float nextAttack;

    Player player;
    GameObject objectToSeek;

    [Header("Bullet Systems")]
    [Space(5)]
    public GameObject bullet;
    public Transform bulletSpawnPos;
    public int bulletSpeed = 150;

    [Header("Enemy SFX")]
    [Space(5)]
    public AudioClip[] audioClips;
    AudioSource audioSource;

    [Header("Enemy VFX")]
    [Space(5)]
    public float hurtAnimationTime;
    public float deathAnimationTime;

    public GameObject damagePopUp;

    Animator animator;
    public Animator weapAnimator;

    Weapon weap;
    string weapName;

    // NavMeshAgent navMeshAgent;
    // public float moveSpeed = 5;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        // navMeshAgent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();

        objectToSeek = GameObject.FindGameObjectWithTag("Player");
        player = FindObjectOfType<Player>();

        weap = GetComponentInChildren<Weapon>();
        weapName = weap.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && health > 0)
        {
            distance = FindPlayer();

            AggroCheck(distance);
        }
        /* else
            navMeshAgent.speed = 0; */
    }

    float FindPlayer()
    {
        float playerDistance = Vector3.Distance(transform.position, objectToSeek.transform.position);

        return playerDistance;
    }

    void AggroCheck(float distance)
    {
        // IDEA- enemy shoots faster the closer player is to the enemy (simulate panic)
        if (distance <= range && health > 0)
        {
            LookAtPlayer();

            if (Time.time >= nextAttack)
                Attack();
        }
        else
            weapAnimator.SetBool(weapName + "Ready", false);

    }

    void LookAtPlayer()
    {
        Vector3 playerPos = new Vector3(objectToSeek.transform.position.x, objectToSeek.transform.position.y, objectToSeek.transform.position.z);

        transform.LookAt(playerPos);

        weapAnimator.SetBool(weapName + "Ready", true);
    }

    void Attack()
    {
        nextAttack = Time.time + 1 / attackSpeed;

        GameObject projectile = Instantiate(bullet, bulletSpawnPos.position, transform.rotation);

        Rigidbody r = projectile.GetComponent<Rigidbody>();
        r.AddForce(projectile.transform.forward * bulletSpeed);

        EnemyBullet eb = projectile.GetComponent<EnemyBullet>();
        eb.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackSpeed;
        }
    }

    public void TakeDamage(int d)
    {
        health -= d;

        if (health > 0)
        { 
            animator.SetBool("isDamaged", true);

            StartCoroutine(Hurt(hurtAnimationTime));

            DamageTextPopUp(d);

            PlaySFX(0);
        }
        else
            Die(d);
    }

    void Die(int damage)
    {
        animator.SetBool("isDead", true);

        if (!isDead)
        {
            DamageTextPopUp(damage);

            player.GainExperience(expAmt);

            isDead = true;
        }

        // PlaySFX(1);

        StartCoroutine("Death", deathAnimationTime);
    }

    public void PlaySFX(int file)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(audioClips[file]);
    }

    public void DamageTextPopUp(int damage)
    {
        var text = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    IEnumerator Hurt(float timer)
    {
        yield return new WaitForSeconds(timer);

        animator.SetBool("isDamaged", false);
    }

    IEnumerator Death(int timer)
    {
        // navMeshAgent.enabled = false;

        yield return new WaitForSeconds(timer);

        Destroy(gameObject);
    }
}
