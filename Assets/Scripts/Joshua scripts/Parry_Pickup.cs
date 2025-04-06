using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry_Pickup : MonoBehaviour
{
    GameObject parryShield;
    GameObject tracker;
    GameObject messageDisplay;
    Message_Display messageScript;
    Collectible_Tracker trackerScript;
    Parry_Shield parryScript;

    void Start()
    {
        parryShield = GameObject.Find("Player shield");
        parryScript = parryShield.GetComponent<Parry_Shield>();

        tracker = GameObject.Find("Collectible Tracker");
        trackerScript = tracker.GetComponent<Collectible_Tracker>();

        messageDisplay = GameObject.Find("Message Display");
        messageScript = messageDisplay.GetComponent<Message_Display>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            parryScript.canParry = true;
            trackerScript.collectibleCount++;
            messageScript.parryAcquired();
            Destroy(gameObject);
        }
    }
}
