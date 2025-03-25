using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("Do not edit")]
    public float damage;
    Player_HP playerHP;

    public bool updateLog = true;

    private void Start()
    {
        playerHP = GameObject.Find("Player").GetComponent<Player_HP>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHP.takeDamage(damage);

            if (updateLog && !playerHP.invulnerable)
                playerHP.meleeDamageTaken += damage;

            updateLog = false;
        }
    }
}
