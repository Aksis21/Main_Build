using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer2 : MonoBehaviour
{
    public bool playerSpotted = false;

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
