using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour 
{
    [Header("Enemy Systems")]
    [Space(5)]
    public float health = 60f;
    public int expAmt = 10;
    bool isDead = false;

    public float moveSpeed = 5;
    public float moveSpeedMultiplier = 1.5f;

    public int range = 10;
    public int aggroRange;
    public int attackRange;
    float distance;

    public float attackSpeed = 1f;
    public int damage = 10;
    float nextAttack;

    Player player;
    GameObject objectToSeek;

    [Header("Enemy SFX")]
    [Space(5)]
    public AudioClip[] audioClips;
    AudioSource audioSource;

    [Header("Enemy VFX")]
    [Space(5)]
    public int hurtAnimationTime;
    public int deathAnimationTime;

    public GameObject damagePopUp;

    NavMeshAgent navMeshAgent;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();

        objectToSeek = GameObject.FindGameObjectWithTag("PlayerBody");
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && navMeshAgent.enabled == true)
        {
            distance = FindPlayer();

            MoveTowardsPlayer(distance);
        }
    }

    float FindPlayer()
    {
        float playerDistance = Vector3.Distance(transform.position, objectToSeek.transform.position);

        return playerDistance;
    }

    void MoveTowardsPlayer(float distance)
    {
        if (distance <= range)
        {
            navMeshAgent.SetDestination(objectToSeek.transform.position);

            if (distance <= aggroRange)
                navMeshAgent.speed = moveSpeed * moveSpeedMultiplier + Random.Range(0, moveSpeed);
            else
                navMeshAgent.speed = moveSpeed;

            if (distance <= attackRange)
            {
                if (Time.time >= nextAttack)
                    Attack();
            }
        }
    }

    void Attack()
    {
        nextAttack = Time.time + 1 / attackSpeed;

        animator.SetBool("isAttacking", true);
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
            navMeshAgent.enabled = false;

            StartCoroutine("Hurt", hurtAnimationTime);

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

        navMeshAgent.enabled = false;

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

    IEnumerator Hurt(int timer)
    {
        yield return new WaitForSeconds(timer);

        if (!isDead)
        {
            navMeshAgent.enabled = true;
            animator.SetBool("isDamaged", false);
        }
    }

    IEnumerator Death(int timer)
    {
        yield return new WaitForSeconds(timer);

        Destroy(gameObject);
    }
}
