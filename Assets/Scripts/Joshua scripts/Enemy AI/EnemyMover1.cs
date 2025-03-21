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
    float timeUntilMove = 0f;
    float countToAttack = 0f;

    [Header("Damage/Speed")]
    public float attackSpeed;
    public float attackDuration;
    float stopAttack = 0f;

    [Header("DO NOT CHANGE")]
    public Transform vision;
    DetectPlayer2 detScript;
    GameObject player;

    bool targetingPlayer = false;
    bool moving = true;
    bool attackingPlayer = false;

    float direction;
    Rigidbody2D rb;
    Vector3 destination;
    Vector3 moveDirection;
    Vector3 attackVector;

    public PolygonCollider2D attackUp;
    public PolygonCollider2D attackRight;
    public PolygonCollider2D attackDown;
    public PolygonCollider2D attackLeft;

    private void Start()
    {
        player = GameObject.Find("Player");
        destination = transform.position;
        rb = GetComponent<Rigidbody2D>();
        detScript = GetComponent<DetectPlayer2>();

        attackUp.enabled = false;
        attackRight.enabled = false;
        attackDown.enabled = false;
        attackLeft.enabled = false;
    }

    void Update()
    {
        if (detScript.playerSpotted) targetingPlayer = true;

        moveDirection = destination - transform.position;
        if (!targetingPlayer) Patrol();
        if (targetingPlayer) ApproachPlayer();

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
            moving = true;
            attackingPlayer = false;
            stopAttack = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (moving)
            rb.velocity = moveDirection * 10000;
        if (attackingPlayer)
            rb.velocity = attackVector * 10000 * attackSpeed;
    }

    void ApproachPlayer()
    {
        destination = player.transform.position;

        Vector3 chooseAttack = player.transform.position - transform.position;
        if (chooseAttack.magnitude > attackRange || attackingPlayer) return;
        chooseAttack.Normalize();

        //Pick attack direction
        if (chooseAttack.y >= Mathf.Abs(chooseAttack.x)) AttackPlayer("Up");
        else if (Mathf.Abs(chooseAttack.y) >= Mathf.Abs(chooseAttack.x)) AttackPlayer("Down");
        else if (chooseAttack.x > Mathf.Abs(chooseAttack.y)) AttackPlayer("Right");
        else if (Mathf.Abs(chooseAttack.x) > Mathf.Abs(chooseAttack.y)) AttackPlayer("Left");
    }

    void AttackPlayer(string attackDirection)
    {
        moving = false;
        countToAttack += Time.deltaTime;

        attackVector = transform.position;

        if (attackDirection == "Up") attackVector.y = transform.position.y + 5;
        else if (attackDirection == "Down") attackVector.y = transform.position.y - 5;
        else if (attackDirection == "Right") attackVector.x = transform.position.x + 5;
        else if (attackDirection == "Left") attackVector.x = transform.position.x - 5;

        if (countToAttack < timeToAttack)
        {
            Debug.Log("attack player");
            return;
        }

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
}
