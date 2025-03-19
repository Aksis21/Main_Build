using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Shield : MonoBehaviour
{
    CircleCollider2D parryCollider;
    SpriteRenderer shieldSprite;

    [Header("Player attributes")]
    public float maxParryEnergy;
    public float parryDrainRate;
    public float dashDrainRate;
    public float energyRechargeRate;

    [Header("Do not change")]
    public GameObject player;
    public bool isParrying = false;
    public bool canUseEnergy = true;
    public float energy;
    public Slider energyDisplay;
    public Image fillColor;

    public Player_Mover playerMover;
    bool isDashing = false;

    private void Start()
    {
        parryCollider = GetComponent<CircleCollider2D>();
        shieldSprite = GetComponent<SpriteRenderer>();
        energy = maxParryEnergy;
        energyDisplay.maxValue = maxParryEnergy;
    }

    void Update()
    {
        transform.position = player.transform.position;

        //Clamp the parry energy between 0 and the max possible parry energy (set in editor)
        energy = Mathf.Clamp(energy, 0, maxParryEnergy);

        //Parry visual and collider both tied to state of "isParrying"
        parryCollider.enabled = isParrying;
        shieldSprite.enabled = isParrying;

        //If the player can parry and presses space, isParrying is true
        if (Input.GetKeyDown(KeyCode.Space) && canUseEnergy)
            isParrying = true;
        //When the player releases space, isParrying is false
        if (Input.GetKeyUp(KeyCode.Space))
            isParrying = false;

        //Drains the parry energy while the player isParrying
        if (isParrying)
            energy -= Time.deltaTime * parryDrainRate;
        //If the player dashes, it drains even MORE energy
        isDashing = playerMover.isDashing;
        if (isDashing)
            energy -= Time.deltaTime * dashDrainRate;
        //If parry energy reaches zero, the parry is turned off and the player CANNOT parry
        if (energy <= 0)
        {
            isParrying = false;
            canUseEnergy = false;
            fillColor.color = new Color(0, 0.7105088f, 1, 0.3921569f);
        }
        //Only once the parry energy has recharged to full can the player parry again if they
        //drained the parry energy fully
        if (energy >= maxParryEnergy)
        {
            canUseEnergy = true;
            fillColor.color = new Color(0, 0.7105088f, 1, 1);
        }

        //Parry energy is constantly recharging
        energy += Time.deltaTime * energyRechargeRate;

        energyDisplay.value = energy;

        /*
        TELEMETRY LOG DATA
        Located here:
        1. Dash/Parry buttons pressed while out of energy
        */

        if (!canUseEnergy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                TelemetryLogger.Log(this, "Attempted parry without energy");
            if (Input.GetKeyDown(KeyCode.LeftShift))
                TelemetryLogger.Log(this, "Attempted dash without energy");
        }
    }
}
