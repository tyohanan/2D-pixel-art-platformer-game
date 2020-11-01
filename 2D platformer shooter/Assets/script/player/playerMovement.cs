using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public int speed = 5;
    public int skalaLoncat = 5;

    public int amountOfJump;
    private int facingDirection = 1;

    public float isGroundColideRadius;
    public float isWallDistance;
    public float wallSlidingSpeed = 2f;
    public float movementForceinAir;
    public float airDragMultiplier = 0.95f;
    //public float variableJumpHeightMultiplier = 0.5f;

    public float wallHopForce;
    public float wallJumpForce;

    private float inputGerak;
    private int amountOfJumpLeft;

    private bool inTheGround;
    private bool inTheWall;
    private bool isFacingRight = true;
    private bool isRunning = false;
    private bool isWallSliding;
    private bool canJump;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    private Rigidbody2D rb;
    private Animator anim;

    public Transform feetPosition;
    public Transform eyePosition;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    void Update()
    {

        adaInput();
        deteksiCollider();
        changeDirection();
        updateAnimation();
        checkIfCanJump();
        checkifWallsliding();
    }

    void FixedUpdate()
    {
        gerakPlayer();
    }

    void checkifWallsliding()
    {
        if (inTheWall && rb.velocity.y <= 0)
        {
            isWallSliding = true;
        }
        else isWallSliding = false;
    }

    private void updateAnimation()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", inTheGround);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void changeDirection()
    {
        if (inputGerak > 0 && isFacingRight == false)
        {
            Flip();
        }
        else if (inputGerak < 0 && isFacingRight == true)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (!isWallSliding)
        {
            facingDirection *= -1;
            transform.Rotate(0, 180, 0);
            isFacingRight = !isFacingRight;
        }
    }

    private void deteksiCollider()
    {
        inTheGround = Physics2D.OverlapCircle(feetPosition.position, isGroundColideRadius, whatIsGround);
        inTheWall = Physics2D.Raycast(eyePosition.position, transform.right, isWallDistance, whatIsGround);
    }

    private void adaInput()
    {
        inputGerak = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

/*        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
*/    }

    private void gerakPlayer()
    {
        if (inTheGround)
        {
            rb.velocity = new Vector2(inputGerak * speed, rb.velocity.y);
        }

        else if (!inTheGround && !isWallSliding && inputGerak != 0)
        {
            Vector2 forcetoAdd = new Vector2(movementForceinAir * inputGerak, 0);
            rb.AddForce(forcetoAdd);

            if (Mathf.Abs(rb.velocity.x) >= speed)
            {
                rb.velocity = new Vector2(speed * inputGerak, rb.velocity.y);
            }
        }
        else if (!inTheGround && !isWallSliding && inputGerak == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        if (Mathf.Abs(inputGerak) > 0)
        {
            isRunning = true;
        }
        else isRunning = false;

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    private void Jump()
    {
        if (canJump && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, skalaLoncat);
            amountOfJumpLeft--;
        }
        else if (isWallSliding && inputGerak == 0 && canJump) //Wall hop
        {
            isWallSliding = false;
            amountOfJumpLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if ((isWallSliding || inTheWall) && inputGerak != 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * inputGerak, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void checkIfCanJump()
    {
        if ((inTheGround && rb.velocity.y <= 0) || isWallSliding)
        {
            amountOfJumpLeft = amountOfJump;
        }

        if (amountOfJumpLeft < 0)
        {
            canJump = false;
        }
        else canJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPosition.position, isGroundColideRadius);
        Gizmos.DrawLine(eyePosition.position, new Vector3(eyePosition.position.x + isWallDistance, eyePosition.position.y, eyePosition.position.z));
    }
}