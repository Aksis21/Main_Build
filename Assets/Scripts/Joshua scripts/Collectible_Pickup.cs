using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Pickup : MonoBehaviour
{
    GameObject tracker;
    GameObject messageDisplay;
    Message_Display messageScript;
    Collectible_Tracker trackerScript;

    private void Start()
    {
        tracker = GameObject.Find("Collectible Tracker");
        trackerScript = tracker.GetComponent<Collectible_Tracker>();

        messageDisplay = GameObject.Find("Message Display");
        messageScript = messageDisplay.GetComponent<Message_Display>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trackerScript.collectibleCount++;

            if (trackerScript.collectibleCount == trackerScript.collectibleTotal)
                messageScript.allDone();

            Destroy(gameObject);
        }
    }
}
