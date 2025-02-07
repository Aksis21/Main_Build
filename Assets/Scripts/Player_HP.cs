using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HP : MonoBehaviour
{
    public float HP;
    public float iFrameTime;
    float invulTimer = 0;

    public float hazardDamage;
    bool inHazard = false;

    void Update()
    {
        invulTimer += Time.deltaTime;

        if (inHazard)
            takeDamage(hazardDamage);
    }

    public void takeDamage(float damageTaken)
    {
        if (invulTimer >= iFrameTime)
        {
            HP -= damageTaken;
            invulTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
            inHazard = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
            inHazard = false;
    }
}
