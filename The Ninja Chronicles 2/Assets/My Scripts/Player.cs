using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Space(5)]
    public float moveSpeed = 11f;
    public float sprintSpeed = 17f;
    float tmpMoveSpeed = 11f;

    public float jumpSpeed = 8f;
    public float sprintJumpSpeed = 21f;
    float tmpJumpSpeed = 17f;
    public float gravity = 20f;

    CharacterController charController;
    Vector3 moveDirection;
    Transform playerBody;
    // public float rotateSpeed = 0.15f;

    [Header("Basic Attack System")]
    [Space(5)]
    public int power = 10;
    public int throwForce = 40;
    public float attackSpeed = 5;
    float nextAttack = 0f;
    public GameObject normalWeaponPrefab;
    public Transform throwPoint;

    [Header("Special Attack System")]
    [Space(5)]
    public Transform explosionStarThrowPoint;
    public GameObject explosionStarPrefab;
    public float explosionStarManaCost = 10;

    public Transform damnationStarThrowPoint;
    public GameObject damnationStarPrefab;
    public float damnationStarManaCost = 50;
    int damnationCounter = 4;

    [Header("Player Stats")]
    [Space(5)]
    public float maxHP = 100;
    public float currentHP = 100;
    public float maxMana = 100;
    public float currentMana = 100;
    public float manaRecovery = 0.5f;
    public int level = 1;
    public int maxLevel = 20;
    public int maxExperience = 100;
    public int experience = 0;

    [Header("Player SFX")]
    [Space(5)]
    public AudioClip[] clips;
    AudioSource audioSource;

    [Header("Player HUD")]
    [Space(5)]
    public PlayerHUD playerHUD;
    Upgrades upgrades;

    [Header("Player VFX")]
    [Space(5)]
    public ParticleSystem powerParticles;
    public ParticleSystem attackSpeedParticles;
    public ParticleSystem rangeParticles;

    public GameObject damagePopUp;

    TrailRenderer trailRend;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        upgrades = FindObjectOfType<Upgrades>();

        Cursor.visible = true;

        trailRend = GetComponent<TrailRenderer>();
        tmpMoveSpeed = moveSpeed;
        tmpJumpSpeed = jumpSpeed;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            Movement();

            ManaRecovery();

            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextAttack)
                nextAttack = NormalAttack(nextAttack);

            else if (currentMana > explosionStarManaCost && Input.GetKey(KeyCode.Mouse1) && Time.time >= nextAttack)
                nextAttack = StarOfExplosion(nextAttack);

            else if (currentMana > damnationStarManaCost && Input.GetKey(KeyCode.Mouse2) && (Time.time) >= nextAttack)
                nextAttack = StarOfDamnation(nextAttack);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                power += 10;
                upgrades.PowerUpgrade();
                upgrades.TurnOffUpgrades();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                throwForce += 20;
                upgrades.RangeUpgrade();
                upgrades.TurnOffUpgrades();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                attackSpeed += 5;
                upgrades.AttackSpeedUpgrade();
                upgrades.TurnOffUpgrades();
            }
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentMana >= 0)
        {
            currentMana -= manaRecovery;

            moveSpeed = sprintSpeed;
            jumpSpeed = sprintJumpSpeed;

            trailRend.emitting = true;
        }
        else
        {
            moveSpeed = tmpMoveSpeed;
            jumpSpeed = tmpJumpSpeed;

            trailRend.emitting = false;
        }

        if (charController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            moveDirection *= moveSpeed;

            if (Input.GetKey(KeyCode.Space))
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(clips[0]);

                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            moveDirection.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
            moveDirection.z = Input.GetAxisRaw("Vertical") * moveSpeed;
        }

        Rotation(moveDirection);

        moveDirection.y -= gravity * Time.unscaledDeltaTime;

        charController.Move(moveDirection * Time.unscaledDeltaTime);
    }

    void Rotation(Vector3 moveDirection)
    {
        Vector3 mouse = Input.mousePosition;

        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Vector3 lookhere = hit.point;
            transform.LookAt(lookhere);
            transform.rotation *= Quaternion.Euler(0, -90, 0);
        }
    }

    void ManaRecovery()
    {
        if (currentMana <= maxMana)
            currentMana += manaRecovery;

        playerHUD.UpdateMana();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        playerHUD.UpdateHP();

        // play SFX
        // play VFX (red screen flash)

        if (currentHP <= 0)
        {
            SceneManager.LoadScene("Game1");
        }    
    }

    public void DamageTextPopUp(int damage)
    {
        var text = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    float NormalAttack(float nextAttack)
    {
        nextAttack = Time.time + 1 / attackSpeed;

        GameObject weapon = Instantiate(normalWeaponPrefab, throwPoint.transform.position, transform.rotation);

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(throwPoint.right * throwForce, ForceMode.VelocityChange);
        else
            Debug.Log("Normal weapon does not contain a Rigidbody!");

        int i = Random.Range(1, 3);
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clips[i]);

        return nextAttack;
    }

    float StarOfExplosion(float nextAttack)
    {
        nextAttack = Time.time + 1 / attackSpeed;
        currentMana -= explosionStarManaCost;
        playerHUD.UpdateMana();

        GameObject weapon = Instantiate(explosionStarPrefab, throwPoint.transform.position, transform.rotation);

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(throwPoint.right * throwForce, ForceMode.VelocityChange);
        else
            Debug.Log("Normal weapon does not contain a Rigidbody!");

        int i = Random.Range(1, 3);
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clips[i]);

        return nextAttack;
    }

    float StarOfDamnation(float nextAttack)
    {
        nextAttack = Time.time + 1 / attackSpeed;
        currentMana -= damnationStarManaCost;
        playerHUD.UpdateMana();

        StartCoroutine("SpawnDamnationStars");

        return nextAttack;
    }

    IEnumerator SpawnDamnationStars()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 25 + level * 5; j++)
            {
                Vector3 newPos = new Vector3(throwPoint.transform.position.x + Random.Range(-10 - level, 10 + level), damnationStarThrowPoint.transform.position.y + Random.Range(0, 5),
                        damnationStarThrowPoint.transform.position.z + Random.Range(-10 - level, 10 + level));

                Vector3 newRot = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

                GameObject weapon = Instantiate(damnationStarPrefab, newPos, Quaternion.Euler(newRot));

                Rigidbody rb = weapon.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(throwPoint.right * throwForce, ForceMode.VelocityChange);
                }
                else
                    Debug.Log("Normal weapon does not contain a Rigidbody!");

                NormalStar n = weapon.GetComponent<NormalStar>();
            }

            int k = Random.Range(1, 3);
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clips[k]);

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void GainExperience(int exp)
    {
        if (level >= maxLevel)
            exp = 0;
        else
        {
            experience += exp;

            if (experience >= maxExperience)
                LevelUp();

            playerHUD.UpdateExperience();
        }
    }

    public void LevelUp()
    {
        level += 1;

        experience -= maxExperience;
        maxExperience += 50;

        playerHUD.UpdateLevel();
        upgrades.DisplayUpgrades();
    }
}
