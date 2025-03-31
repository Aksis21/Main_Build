using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover1 : MonoBehaviour
{
    [Header("Timing")]
    public float timeToForgor;
    public float attackRange;
    public float timeToAttack;
    float countToForgor = 0f;
    float timeUntilMove = float.PositiveInfinity;
    float countToAttack = 0f;

    [Header("Damage/Speed")]
    public float HP;
    float hitTimer = float.PositiveInfinity;
    SpriteRenderer sprite;
    public float patrolSpeed;
    public float chaseSpeed;
    public float attackSpeed;
    public float attackDuration;
    public float damage;
    public float cooldownTime;
    float timeToCooldown = 0f;
    float stopAttack = 0f;

    [Header("DO NOT CHANGE")]
    public Transform vision;
    DetectPlayer2 detScript;
    GameObject player;
    Player_HP playerScript;

    public float disableRange;
    public float currentDisableRange;

    bool targetingPlayer = false;
    bool moving = true;
    bool attackingPlayer = false;
    bool startAttacking = false;
    bool cooldown = false;

    float direction;
    Rigidbody2D rb;
    Vector3 destination;
    Vector3 moveDirection;
    Vector3 attackVector;
    Vector3 chosenAttackVector;
    string attackDirection;

    MeleeAttack dmgDet;

    //FOR ANIMATION PURPOSES ONLY.
    Animator animator;
    public float verticalAnim = 0f;
    public float horizontalAnim = 0f;

    public PolygonCollider2D attackUp;
    public PolygonCollider2D attackRight;
    public PolygonCollider2D attackDown;
    public PolygonCollider2D attackLeft;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player_HP>();

        destination = transform.position;
        rb = GetComponent<Rigidbody2D>();
        detScript = GetComponent<DetectPlayer2>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        dmgDet = GetComponentInChildren<MeleeAttack>();
        dmgDet.damage = damage;

        attackUp.enabled = false;
        attackRight.enabled = false;
        attackDown.enabled = false;
        attackLeft.enabled = false;
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
        verticalAnim = rb.velocity.y;
        horizontalAnim = rb.velocity.x;

        if (!startAttacking)
        {
            animator.SetFloat("Horizontal", horizontalAnim);
            animator.SetFloat("Vertical", verticalAnim);
        }

        else if (startAttacking)
        {
            animator.SetFloat("Horizontal", chosenAttackVector.x);
            animator.SetFloat("Vertical", chosenAttackVector.y);
        }

        animator.SetBool("Chase", targetingPlayer);
        animator.SetBool("Attack", startAttacking);
        animator.SetBool("Cooldown", cooldown);

        //END OF ANIMATION STUFF

        if (timeToCooldown > cooldownTime)
        {
            timeToCooldown = 0;
            cooldown = false;
        }
        if (cooldown)
        {
            timeToCooldown += Time.deltaTime;
            return;
        }

        if (detScript.playerSpotted) targetingPlayer = true;

        moveDirection = destination - transform.position;
        if (!targetingPlayer) Patrol();

        if (targetingPlayer && !startAttacking) ApproachPlayer();
        if (startAttacking) AttackPlayer();

        if (!detScript.playerSpotted)
            countToForgor += Time.deltaTime;
        if (detScript.playerSpotted)
            countToForgor = 0f;
        if (countToForgor >= timeToForgor)
        {
            Debug.Log("Goodbye");
            targetingPlayer = false;
            countToForgor = 0f;
        }

        if (attackingPlayer)
            stopAttack += Time.deltaTime;
        if (stopAttack > attackDuration)
        {
            attackUp.enabled = false;
            attackRight.enabled = false;
            attackDown.enabled = false;
            attackLeft.enabled = false;

            moving = true;
            startAttacking = false;
            attackingPlayer = false;
            stopAttack = 0f;

            cooldown = true;
        }
    }

    private void FixedUpdate()
    {
        //DISABLE AT DISABLE RANGE
        if (currentDisableRange > disableRange) return;

        if (cooldown) return;

        moveDirection.Normalize();

        if (moving && !targetingPlayer)
            rb.velocity = moveDirection * 10000 * patrolSpeed;
        if (moving && targetingPlayer)
            rb.velocity = moveDirection * 10000 * chaseSpeed;
        if (attackingPlayer)
            rb.velocity = chosenAttackVector * 10000 * attackSpeed;
    }

    void ApproachPlayer()
    {
        destination = player.transform.position;

        //Rotate vision to always follow the player
        float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, angle + 90);

        Vector3 chooseAttack = player.transform.position - transform.position;
        if (chooseAttack.magnitude > attackRange || attackingPlayer)
        {
            moving = true;
            countToAttack = 0f;
            return;
        }
        chooseAttack.Normalize();

        //Pick attack direction
        if (chooseAttack.y >= Mathf.Abs(chooseAttack.x)) attackDirection = "Up";
        else if (Mathf.Abs(chooseAttack.y) >= Mathf.Abs(chooseAttack.x)) attackDirection = "Down";
        else if (chooseAttack.x > Mathf.Abs(chooseAttack.y)) attackDirection = "Right";
        else if (Mathf.Abs(chooseAttack.x) > Mathf.Abs(chooseAttack.y)) attackDirection = "Left";

        startAttacking = true;
    }

    void AttackPlayer()
    {
        Debug.Log("Attacking " + attackDirection.ToString());

        moving = false;
        countToAttack += Time.deltaTime;

        attackVector = transform.position;

        if (attackDirection == "Up") attackVector.y += 5;
        else if (attackDirection == "Down") attackVector.y -= 5;
        else if (attackDirection == "Right") attackVector.x += 5;
        else if (attackDirection == "Left") attackVector.x -= 5;

        chosenAttackVector = attackVector - transform.position;

        if (countToAttack < timeToAttack) return;

        //FOR TELEMETRY
        dmgDet.updateLog = true;

        if (attackDirection == "Up") attackUp.enabled = true;
        else if (attackDirection == "Down") attackDown.enabled = true;
        else if (attackDirection == "Right") attackRight.enabled = true;
        else if (attackDirection == "Left") attackLeft.enabled = true;

        Debug.Log("attack player");
        countToAttack = 0f;
        attackingPlayer = true;
    }

    void Patrol()
    {
        timeUntilMove += Time.deltaTime;

        if (timeUntilMove > Random.Range(2, 3))
        {
            direction = Random.Range(1, 5);
            int moveDistance = Random.Range(1, 4);

            if (direction == 1)
            {
                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 180);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            }

            if (direction == 2)
            {
                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 90);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
            }

            if (direction == 3)
            {
                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 0);
                destination = new Vector2(transform.position.x, transform.position.y - moveDistance);
            }

            if (direction == 4)
            {
                vision.eulerAngles = new Vector3(vision.rotation.x, vision.rotation.y, 270);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
            }

            Debug.Log("move");
            timeUntilMove = 0f;
        }
    }

    public void Hit()
    {
        HP -= 1;
        hitTimer = 0f;
    }
}
