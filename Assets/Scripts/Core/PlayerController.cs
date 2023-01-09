using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player components
    private Rigidbody2D rb;

    // Parent object
    public GameObject playerObject;

    // Directional variables
    public float isFacingLeft = -1;

    // Navigation variables
    public float speed;
    private float moveInput;

    // Prism's state variables
    private bool isMoving = false;
    private bool isGrounded;

    // Jupming variables
    public float jumpForce;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Animation variables
    private Animator anim;

    // Magnetic attack variables
    public GameObject directForce;
    public GameObject radialForce;
    private PointEffector2D radialForcePointEffector;
    private bool isDirectForcePushing = false;

    // Input variables
    private bool isKeyPressed = false;

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
        isMoving = IsMoving();

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

    bool IsMoving()
    {
        moveInput = Input.GetAxis("Horizontal");
        if (moveInput != 0)
        {
            return true;
        } else 
        {
            return false;
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
        if (Input.GetKeyDown(KeyCode.Space) && !isKeyPressed)
        {
            isKeyPressed = true;
            anim.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpForce;
        }
        isKeyPressed = false;
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