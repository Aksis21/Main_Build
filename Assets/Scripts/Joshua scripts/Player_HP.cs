using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Player_Mover;

public class Player_HP : MonoBehaviour
{
    [Header("Player attributes")]
    public float HP;
    public float iFrameTime;
    float invulTimer = 0;

    [Header("Set damage values")]
    public float hazardDamage;
    public float projectileDamage;

    [Header("Do not change")]
    public bool invulnerable;
    bool inHazard = false;

    SpriteRenderer playerRenderer;
    Color damageLerp = Color.white;
    bool showDamage = false;

    public Slider healthBar;
    public Parry_Shield parryManager;
    public TextMeshProUGUI winLossText;
    public Image fade;
    public Button restartBut;
    public Image restartCol;
    public TextMeshProUGUI restartText;

    public bool dead = false;
    public bool win = false;

    //STRICTLY FOR TELEMETRY DATA, DO NOT USE ELSEWHERE
    float damageTakenInHazard = 0f;
    float timeOutOfHazard = 0f;
    bool logHazardDamage = false;
    bool logTotalDamage = true;
    [Header("TELEMETRY LOG VARIABLES")]
    public float hazardDamageTaken = 0f;
    public float projectileDamageTaken = 0f;

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        healthBar.maxValue = HP;
        winLossText.text = "";
        restartBut.enabled = false;
        restartText.text = "";
    }

    [System.Serializable]

    public struct DeathEventData
    {
        public float hazardDMG;
        public float projectileDMG;
    }

    void Update()
    {
        /*
        TELEMETRY LOG DATA
        Located here:
        3. How much damage does the player take each instance of being in the hazard?
        5. How much damage was taken from projectiles vs. water hazard?
        */

        //Upon exiting the hazard, start counting up.
        if (!inHazard)
            timeOutOfHazard += Time.deltaTime;
        //If re-enter hazard, reset log time.
        if (inHazard)
            timeOutOfHazard = 0f;

        //If the script is primed to log and the player exits hazard for more than 0.5s, log damage taken from hazard.
        if (timeOutOfHazard >= 0.5f && logHazardDamage)
        {
            if (damageTakenInHazard > 0)
                TelemetryLogger.Log(this, "Damage taken in hazard", damageTakenInHazard);
            damageTakenInHazard = 0f;
            logHazardDamage = false;
        }

        if (dead && logTotalDamage)
        {
            var data = new DeathEventData()
            {
                hazardDMG = hazardDamageTaken,
                projectileDMG = projectileDamageTaken
            };
            TelemetryLogger.Log(this, "Damage taken from each source", data);
            logTotalDamage = false;
        }
        //
        //END OF TELEMETRY
        //

        //Update health bar
        healthBar.value = HP;

        if (HP <= 0)
            dead = true;

        if (win) return;

        //TRIGGER DEATH SCREEN
        if (dead)
        {
            winLossText.text = "You died";
            dead = true;
            fade.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            restartBut.enabled = true;
            restartCol.color = new Color(1, 1, 1, 1);
            restartText.text = "Restart";
            return;
        }

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

            //LOG TELEMETRY DATA
            if (inHazard)
            {
                //Tracks damage taken in hazard PER INSTANCE, IS RESET ON EXITING HAZARD
                damageTakenInHazard++;

                //Tracks TOTAL DAMAGE TAKEN BY HAZARD
                hazardDamageTaken += damageTaken;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            inHazard = true;

            //Prime script to log hazard damage taken.
            logHazardDamage = true;
        }

        //TRIGGER WIN SCREEN
        if (collision.gameObject.tag == "End")
        {
            winLossText.text = "You win!";
            win = true;
            fade.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            restartBut.enabled = true;
            restartCol.color = new Color(1, 1, 1, 1);
            restartText.text = "Restart";

            //TELEMETRY LOG HEALTH AT END
            TelemetryLogger.Log(this, "Health at end of game", HP);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
            inHazard = false;
    }
}
