using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Mover : MonoBehaviour
{
    public float moveSpeed;
    public float dashTimer;
    public float dashAmount;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
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

        //For dash mechanic
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = currentSpeed * dashAmount;
            isDashing = true;
        }

        if (isDashing)
            timer += Time.deltaTime;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard" && !isDashing)
        {
            inHazard = true;
            moveSpeed /= 2;
            currentSpeed = moveSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard" && inHazard)
        {
            inHazard = false;
            moveSpeed *= 2;
            currentSpeed = moveSpeed;
        }
    }
}
