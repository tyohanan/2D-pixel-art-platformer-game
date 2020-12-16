using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlyingEnemyController : MonoBehaviour
{
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }

    private State currentState;


    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        maxHealth,
        knockbackDuration,
        lastTouchDamageTime,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;
    [SerializeField]
    private Transform
        groundCheck,
        wallCheck,
        touchDamageCheck;
    [SerializeField]
    private LayerMask
        whatIsGround,
        whatIsPlayer;
    [SerializeField]
    private Vector2 knockbackSpeed;

    [Header("Player Dead Data")]
    public Vector2 knockbackDeathSpeed;
    public float deathTorque = 1f;

    [SerializeField]
    private GameObject
        hitParticle,
        deathChunkParticle,
        deathBloodParticle;

    private float
        currentHealth,
        knockbackStartTime;

    private AttackDetails attackDetails;

    private int
        facingDirection,
        damageDirection;

    private Vector2
        movement,
        touchDamageBotLeft,
        touchDamageTopRight;

    private bool
        groundDetected,
        wallDetected;

    private GameObject alive, headEnemy, bodyEnemy;
    private Rigidbody2D aliveRb, headrb, bodyrb;
    private Animator aliveAnim;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveAnim = alive.GetComponent<Animator>();

        bodyEnemy = transform.Find("bodyEnemy").gameObject;
        headEnemy = transform.Find("headEnemy").gameObject;

        aliveRb = alive.GetComponent<Rigidbody2D>();
        bodyrb = bodyEnemy.GetComponent<Rigidbody2D>();
        headrb = headEnemy.GetComponent<Rigidbody2D>();

        alive.SetActive(true);
        headEnemy.SetActive(false);
        bodyEnemy.SetActive(false);

        currentHealth = maxHealth;
        facingDirection = 1;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    //--WALKING STATE--------------------------------------------------------------------------------

    private void EnterMovingState()
    {

    }

    private void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        if (!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }

    private void ExitMovingState()
    {

    }

    //--KNOCKBACK STATE-------------------------------------------------------------------------------

    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("Knockback", false);
    }

    //--DEAD STATE---------------------------------------------------------------------------------------

    private void EnterDeadState()
    {
        //Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        //Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        DeadPlayer();

    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    //--OTHER FUNCTIONS--------------------------------------------------------------------------------
    public virtual void DeadPlayer()
    {
        alive.SetActive(false);
        bodyEnemy.SetActive(true);
        headEnemy.SetActive(true);

        headEnemy.transform.position = alive.transform.position;
        bodyEnemy.transform.position = alive.transform.position;

        bodyrb.velocity = knockbackDeathSpeed;
        bodyrb.AddTorque(deathTorque * -facingDirection, ForceMode2D.Impulse);
        headrb.AddTorque(deathTorque * -facingDirection, ForceMode2D.Impulse);
    }

    private void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (attackDetails.position.x > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //Hit particle

        if (currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else if (currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            attackDetails.damageAmount = touchDamage;
            attackDetails.position.x = alive.transform.position.x;
            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);

            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }

}
