using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    GameObject player;
    Player_HP playerHP;

    void Start()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<Player_HP>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player)
            playerHP.takeDamage(playerHP.spikeDamage);
    }
}
