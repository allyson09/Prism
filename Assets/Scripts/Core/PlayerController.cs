using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player components
    private Rigidbody2D rb;

    // Parent object
    public GameObject playerObject;

    // Directional
    public float isFacingLeft = -1;

    // Navigation
    public float speed;
    private float moveInput;

    // Prism's state
    private bool isMoving = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isGrounded;

    // Jupming
    public float jumpForce;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Animation
    private Animator anim;

    // Magnetic attack
    public GameObject directForce;
    public GameObject radialForce;
    private PointEffector2D radialForcePointEffector;
    private bool isDirectForcePushing = false;

    // Input
    // private bool isKeyPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = playerObject.GetComponent<Rigidbody2D>();
        // radialForcePointEffector = radialForce.GetComponent<PointEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StateCheck();
        PlayerActions();
    }

    void StateCheck()
    {
        // Check if Prism is moving left or right
        IsMoving();

        // Check if Prism is airborne
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    void PlayerActions()
    {
        // Make Prism face the direction she's moving
        FlipPlayerSprite();

        // Make Prism move left or right while grounded
        Move();

        // Make Prism jump
        Jump();

        // Make Prism attack
        Attack();
    }

    void IsMoving()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveInput = -1;
            isMovingLeft = true;
            isMovingRight = false;
            isMoving = true;
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveInput = 1;
            isMovingRight = true;
            isMovingLeft = false;
            isMoving = true;
        } else if ((Input.GetKeyUp(KeyCode.LeftArrow) && isMovingLeft) || (Input.GetKeyUp(KeyCode.RightArrow) && isMovingRight))
        {
            isMoving = false;
            moveInput = 0;
        }
    }

    void FlipPlayerSprite()
    {
        // If Prism is facing the wrong direction flip her
        if (isFacingLeft < 0 && moveInput > 0 || isFacingLeft > 0 && moveInput < 0)
        {
            isFacingLeft *= -1;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    void Move()
    {
        // Update Prism's movement velocity
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // If Prism is grounded and moving, activate her run animation and deactivate her jump animation
        if (isGrounded)
        {
            anim.SetBool("isRunning", isMoving);
        }
        anim.SetBool("isJumping", !isGrounded);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // isKeyPressed = true;
            anim.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpForce;
        }
        // isKeyPressed = false;
    }
    
    void Attack()
    {
        // Direct force attack
        // if(Input.GetKey(KeyCode.D) && !isDirectForcePushing)
        // {
        //     isDirectForcePushing = true;
        //     directForce.SetActive(true);

        // } else if (!Input.GetKey(KeyCode.D) && isDirectForcePushing)
        // {
        //     directForce.SetActive(false);
        //     isDirectForcePushing = false;
        // }

        // Radial force attack
        // if(Input.GetKey(KeyCode.E) && isKeyPressed)
        // {
        //     isKeyPressed = true;
        //     radialForce.SetActive(true);
        // } else if (!Input.GetKey(KeyCode.E))
        // {
        //     radialForce.SetActive(false);
        // }
    }
}