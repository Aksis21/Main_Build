using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer2 : MonoBehaviour
{
    public bool playerSpotted = false;

    GameObject player;
    Player_HP playerScript;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player_HP>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerScript.detected = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerSpotted = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerSpotted = false;
    }
}
