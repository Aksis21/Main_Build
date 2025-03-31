using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyMover2 : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float timeToForgor;
    public float HP;
    public float moveSpeed;
    public float attackRange;
    public float timeToAttack;
    public float timeBetweenAttacks;
    public float cooldownTime;
    float timeToCooldown = 0f;
    float timeToSecondAttack;
    float timeToThirdAttack;
    float attackTimer = 0f;
    float countToForgor = 0f;

    [Header("DO NOT CHANGE")]
    public Transform vision;
    public GameObject projectile;
    DetectPlayer2 detScript;
    GameObject player;
    Player_HP playerScript;

    public float disableRange;
    public float currentDisableRange;
    
    float timeUntilLook = 0f;
    float hitTimer = float.PositiveInfinity;
    SpriteRenderer sprite;
    int direction;
    bool targetingPlayer = false;
    bool moving = false;
    bool attacking = false;
    bool firstAttack = false;
    bool secondAttack = false;
    bool cooldown = false;
    Vector3 destination;
    Vector3 moveDirection;
    Rigidbody2D rb;

    //FOR ANIMATION PURPOSES ONLY.
    Animator animator;
    public float verticalAnim = 0f;
    public float horizontalAnim = 0f;

    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player_HP>();

        rb = GetComponent<Rigidbody2D>();
        detScript = GetComponent<DetectPlayer2>();
        destination = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        timeToSecondAttack = timeToAttack + timeBetweenAttacks;
        timeToThirdAttack = timeToSecondAttack + timeBetweenAttacks;
    }

    void Update()
    {
        //DISABLE ENEMY PAST DISABLE RANGE
        Vector3 disable = player.transform.position - transform.position;
        currentDisableRange = disable.magnitude;
        if (currentDisableRange > disableRange) return;

        hitTimer += Time.deltaTime;
        sprite.color = Color.Lerp(Color.red, Color.white, hitTimer);
        if (HP <= 0) Destroy(gameObject);

        //HORDE DETECTION
        Vector3 alertDetect = player.transform.position - transform.position;
        if (playerScript.detected && alertDetect.magnitude < 5) targetingPlayer = true;

        //FOR ANIMATION
        animator.SetBool("Attack", attacking);
        animator.SetBool("Cooldown", cooldown);

        if (timeToCooldown > cooldownTime)
        {
            firstAttack = false;
            secondAttack = false;
            attacking = false;
            cooldown = false;
            timeToCooldown = 0f;
            attackTimer = 0f;
        }
        if (cooldown)
        {
            timeToCooldown += Time.deltaTime;
            return;
        }

        moveDirection = destination - transform.position;

        if (detScript.playerSpotted) targetingPlayer = true;
        if (!targetingPlayer) Patrol();
        if (targetingPlayer && !attacking) ApproachPlayer();

        animator.SetFloat("Horizontal", horizontalAnim);
        animator.SetFloat("Vertical", verticalAnim);

        if (attacking)
        {
            moving = false;
            AttackPlayer();
        }

        if (!detScript.playerSpotted)
            countToForgor += Time.deltaTime;
        if (detScript.playerSpotted)
            countToForgor = 0f;
        if (countToForgor >= timeToForgor)
        {
            moving = false;
            targetingPlayer = false;
            countToForgor = 0f;
        }
    }

    private void FixedUpdate()
    {
        //DISABLE AT DISABLE RANGE
        if (currentDisableRange > disableRange) return;

        if (!moving) return;

        moveDirection.Normalize();

        rb.velocity = moveDirection * 10000 * moveSpeed;
    }

    void ApproachPlayer()
    {
        //FOR ANIMATION
        verticalAnim = rb.velocity.y;
        horizontalAnim = rb.velocity.x;

        moving = true;
        destination = player.transform.position;

        float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, angle + 90);

        Vector3 distToPlayer = player.transform.position - transform.position;
        if (distToPlayer.magnitude > attackRange) return;
        attacking = true;
    }

    void AttackPlayer()
    {
        Vector3 facePlayer = player.transform.position - transform.position;
        //FOR ANIMATION
        verticalAnim = facePlayer.y;
        horizontalAnim = facePlayer.x;

        attackTimer += Time.deltaTime;

        if (attackTimer < timeToAttack) return;

        if (!firstAttack)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            firstAttack = true;
        }

        if (attackTimer < timeToSecondAttack) return;

        if (!secondAttack)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            secondAttack = true;
        }

        if (attackTimer < timeToThirdAttack) return;

        Instantiate(projectile, transform.position, Quaternion.identity);
        cooldown = true;
    }

    void Patrol()
    {
        timeUntilLook += Time.deltaTime;

        if (timeUntilLook > Random.Range(2,3))
        {
            direction = Random.Range(1, 5);

            if (direction == 1)
            {
                //FOR ANIMATION
                verticalAnim = 1;
                horizontalAnim = 0;

                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 180);
            }
            if (direction == 2)
            {
                //FOR ANIMATION
                verticalAnim = 0;
                horizontalAnim = 1;

                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 90);
            }
            if (direction == 3)
            {
                //FOR ANIMATION
                verticalAnim = -1;
                horizontalAnim = 0;

                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 0);
            }
            if (direction == 4)
            {
                //FOR ANIMATION
                verticalAnim = 0;
                horizontalAnim = -1;

                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 270);
            }

            timeUntilLook = 0f;
        }
    }

    public void Hit()
    {
        HP -= 1;
        hitTimer = 0f;
    }
}
