using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HP : MonoBehaviour
{
    public float HP;
    public float iFrameTime;
    float invulTimer = 0;

    public float hazardDamage;
    public bool invulnerable;
    bool inHazard = false;

    void Update()
    {
        //invulTimer keeps climbing. If reset to zero, the player receives "iFrames" and is temporarily invulnerable.
        if (invulTimer <= 0)
            invulnerable = true;
        invulTimer += Time.deltaTime;
        if (invulTimer >= iFrameTime)
        {
            invulnerable = false;

            //If the player is dashing, they are temporarily invulnerable.
            invulnerable = gameObject.GetComponent<Player_Mover>().isDashing;
        }
            
        if (inHazard)
            takeDamage(hazardDamage);
    }

    public void takeDamage(float damageTaken)
    {
        if (!invulnerable)
        {
            //The player takes damage according to how much the source tells them to.
            HP -= damageTaken;

            //The player has temporary invulnerability after taking damage.
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
