using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Mover : MonoBehaviour
{
    public float moveSpeed;
    public float hazardSpeed;
    public float dashTimer;
    public float dashSpeed;

    float currentSpeed;
    float timer = 0;
    public bool isDashing = false;
    Rigidbody2D rb;
    Vector2 input;

    bool inHazard = false;

    //floats for animator
    float verticalAnim = 0;
    float horizontalAnim = 0;
    Animator animator;

    Vector3 lastLogPosition;

    public Parry_Shield energyManager;
    bool canUseEnergy = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    [System.Serializable]

    public struct DashEventData
    {
        public Vector3 playerPos;
        public bool isPlayerInHazard;
    }

    void Update()
    {
        //Determine player's energy to see if they can dash or not
        canUseEnergy = energyManager.canUseEnergy;

        if (Vector3.Distance(transform.position, lastLogPosition) > 1f)
        {
            TelemetryLogger.Log(this, "Move", transform.position);
            lastLogPosition = transform.position;
        }

        //Getting player input
        if (!isDashing)
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
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && canUseEnergy)
        {
            currentSpeed = dashSpeed;
            isDashing = true;

            var data = new DashEventData()
            {
                playerPos = transform.position,
                isPlayerInHazard = inHazard
            };

            TelemetryLogger.Log(this, "Dash", data);
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard" && inHazard)
        {
            inHazard = false;
            currentSpeed = moveSpeed;
        }
    }
}
