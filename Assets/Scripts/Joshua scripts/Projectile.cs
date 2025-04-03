using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("DO NOT CHANGE ANYTHING")]
    public float moveSpeed = 1f;
    GameObject player;
    Player_HP playerHP;
    public Vector3 direction;

    public bool targetPlayer = true;
    bool reflected = false;
    public float invulOnSpawn = 0f;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<Player_HP>();

        if (targetPlayer)
        {
            direction = player.transform.position - transform.position;
            direction.Normalize();
        }
    }

    void Update()
    {
        invulOnSpawn += Time.deltaTime;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" && invulOnSpawn > 0.05f) Destroy(gameObject);
        if (collision.gameObject.tag == "Player")
        {
            playerHP.takeDamage(playerHP.projectileDamage);
            playerHP.projectileDamageTaken += playerHP.projectileDamage;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Parry Shield")
        {
            reflected = true;
            moveSpeed *= 1.5f;
            direction = transform.position - player.transform.position;
            direction.Normalize();
        }

        if (reflected)
        {
            if (collision.gameObject.tag == "MeleeEnemy")
            {
                EnemyMover1 melee = collision.gameObject.GetComponent<EnemyMover1>();
                melee.Hit();
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "RangedEnemy")
            {
                EnemyMover2 ranged = collision.gameObject.GetComponent<EnemyMover2>();
                ranged.Hit();
                Destroy(gameObject);
            }
        }
    }
}
