using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    GameObject player;
    Player_HP playerHP;

    void Start()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<Player_HP>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            playerHP.healthRegen();
            Destroy(gameObject);
        }
    }
}
