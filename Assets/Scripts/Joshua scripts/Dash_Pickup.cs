using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Pickup : MonoBehaviour
{
    GameObject player;
    GameObject tracker;
    GameObject messageDisplay;
    Message_Display messageScript;
    Collectible_Tracker trackerScript;
    Player_Mover moveScript;

    void Start()
    {
        player = GameObject.Find("Player");
        moveScript = player.GetComponent<Player_Mover>();

        tracker = GameObject.Find("Collectible Tracker");
        trackerScript = tracker.GetComponent<Collectible_Tracker>();

        messageDisplay = GameObject.Find("Message Display");
        messageScript = messageDisplay.GetComponent<Message_Display>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveScript.canDash = true;
            trackerScript.collectibleCount++;
            messageScript.dashAcquired();
            Destroy(gameObject);
        }
    }
}
