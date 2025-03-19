using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Mover : MonoBehaviour
{
    [Header("Player attributes")]
    public float moveSpeed;
    public float hazardSpeed;
    public float dashTimer;
    public float dashSpeed;

    float currentSpeed;
    float timer = 0;
    [Header("Do not change")]
    public bool isDashing = false;
    Rigidbody2D rb;
    Vector2 input;

    bool inHazard = false;
    //STRICTLY for telemetry logging, do NOT use elsewhere
    bool logCheckHazard = false;

    //floats for animator
    float verticalAnim = 0;
    float horizontalAnim = 0;
    Animator animator;

    Vector3 lastLogPosition;

    Player_HP playerHP;
    public Parry_Shield energyManager;
    bool canUseEnergy = true;

    bool gameOver = false;

    void Start()
    {
        playerHP = GetComponent<Player_HP>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        //Determine player's energy to see if they can dash or not
        canUseEnergy = energyManager.canUseEnergy;

        //Getting player input
        if (!isDashing && !gameOver)
        {
            input.y = Input.GetAxisRaw("Vertical");

            if (input.y != 0)
                input.x = 0;

            if (input.y == 0)
                input.x = Input.GetAxisRaw("Horizontal");
        }

        //For Animator
        verticalAnim = input.y;
        horizontalAnim = input.x;

        animator.SetFloat("Horizontal", horizontalAnim);
        animator.SetFloat("Vertical", verticalAnim);
        animator.SetBool("Dash", isDashing);

        //Player starts dashing IF they are not already dashing.
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && canUseEnergy && !gameOver)
        {
            currentSpeed = dashSpeed;
            isDashing = true;
        }

        //Begins the Dash timer immediately after the player starts dashing.
        if (isDashing)
            timer += Time.deltaTime;

        //Once the timer has reached the dashTimer limit, they stop dashing, and the timer is reset.
        if (timer > dashTimer)
        {
            isDashing = false;
            currentSpeed = moveSpeed;
            timer = 0;

            /*
            TELEMETRY LOG DATA
            Located here:
            2. Player exits dash in hazard
            */

            if (logCheckHazard)
                TelemetryLogger.Log(this, "Exit dash in hazard");
        }

        //On WIN OR LOSS, DISABLE ALL MOVEMENT.
        if (playerHP.win || playerHP.dead)
        {
            gameOver = true;
            input.x = 0;
            input.y = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = input * currentSpeed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard" && !isDashing)
        {
            inHazard = true;
            currentSpeed = hazardSpeed;
        }

        if (collision.gameObject.tag == "Hazard")
            logCheckHazard = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        logCheckHazard = false;

        if (collision.gameObject.tag == "Hazard" && inHazard)
        {
            inHazard = false;
            currentSpeed = moveSpeed;
        }
    }
}
