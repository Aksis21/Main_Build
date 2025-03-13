using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    public float HP;
    public float iFrameTime;
    float invulTimer = 0;

    public float hazardDamage;
    public bool invulnerable;
    bool inHazard = false;

    SpriteRenderer playerRenderer;
    Color damageLerp = Color.white;
    bool showDamage = false;

    public Slider healthBar;
    public Parry_Shield parryManager;

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        healthBar.maxValue = HP;
    }

    void Update()
    {
        //Update health bar
        healthBar.value = HP;

        //Flash the player red upon taking damage.
        if (showDamage)
            damageLerp = Color.Lerp(Color.red, Color.white, invulTimer);
        playerRenderer.color = damageLerp;

        //invulTimer keeps climbing. If reset to zero, the player receives "iFrames" and is temporarily invulnerable.
        if (invulTimer <= 0)
            invulnerable = true;
        invulTimer += Time.deltaTime;
        if (invulTimer >= iFrameTime)
        {
            invulnerable = false;

            //If the player is dashing, they are temporarily invulnerable.
            invulnerable = gameObject.GetComponent<Player_Mover>().isDashing;

            //If the player is parrying, they are temporarily invulnerable.
            if (parryManager.isParrying)
                invulnerable = true;

            //Stop updating the player color after taking damage.
            showDamage = false;
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

            //Enable the player to flash red, showing damage taken.
            showDamage = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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
