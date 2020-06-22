using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{

    public enum JumpModeToggle
    {
        BUNNY_HOP,
        HIGH_JUMP,
        DOUBLE_JUMP
    }



    [Header("Movement Attributes")]
    public float speed = 12f;           // Player movement speed
    public float dashSpeedMultiplier;   // Multiplying the current player speed, how fast can the player dash
    public float dashLength;            // How long the player can dash
    public float dashCooldown;          // How long before the player can dash again


    [HeaderAttribute("Jumping Attributes")]
    // Jumping attributes
    public JumpModeToggle jumpModeToggle;
    public float gravity = -9.81f;      // Jumping gravity. Higher value = greater force upwards and downwards
    public float fallGravityMultiplyer = 1; // Multiplier determining how fast the player should fall. Modifies gravity during falling velocity
    public float jumpHeight = 3f;
    public float bunnyhopSpeedIncrementAmt = 4;  

    public Transform groundCheck;       // groundcheck object
    public float groundDistance = 0.4f; // distranced before we consider ourselves grounded
    public LayerMask groundMask;        // What is determined as ground
    

    /// Private Variables
    /// -------------------------------------------------------
    private CharacterController controller;

    // Player Movement values
    private Vector2 movement;
    private float defaultSpeed;         // The default player movement speed

    // Player Jump Values
    private float jump;
    private Vector3 velocity;
    private bool isGrounded;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    // Player Dashing Values
    private float inputDash;                 // For getting the value for dashing
    private Vector2 dashDirection;      // x = left/right, y = z, forward/backward
    private bool isDashing;             // If the player is currently dashing
    private float dashingTimer;         // Used for determining how far the player will dash in time
    private float dashCooldownTimer;    // Timer for determining when to cooldown the dash 


    #region Unity Input Passthrough
    public void movementVectorInpuPassthrough(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>(); 
        Debug.Log("Movement value: " + movement.ToString());
    }

    public void jumpInputPassthrough(InputAction.CallbackContext context)
    {
        jump = context.ReadValue<float>();
        Debug.Log("Jump: " + jump.ToString());
    }
    
    public void dashInputPassthrough(InputAction.CallbackContext context)
    {
        inputDash = context.ReadValue<float>();
        Debug.Log("Dashing Input: " + inputDash.ToString());
    }
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultSpeed = speed;
    }

    // Update is called once per frame
    private void Update()
    {
        // check if grounded
        // create sphere based on groundcheck position, and groundDistance as radius. groundMask determines what is can hit.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        move();
        dash();
        Jump();
    }

    private void move()
    {
        // If the player is grounded and the player's downward velocity means it was falling before but now landed, reset velocity
        if(isGrounded && velocity.y < 0)  
        {
            // if set to zero, might register before player actually hits ground. 
            // small enough to forve player to the ground far enough
            velocity.y = -2f;
        }


        // If dashing, force move the player depending on the direction the player last pressed, if
        if(isDashing && dashingTimer > 0)
        {
            float x = dashDirection.x;
            float z = dashDirection.y;

            
            // creates vector 3 for moving player with character controller.
            // left/right, transform.right uses player facing direction, and moves it's right
            // forward/backward, transform.forward  using facing direction, moves forward
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * (speed * dashSpeedMultiplier) * Time.deltaTime);

            // Decrement dashing values
            dashingTimer -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;

            // If the player is in air, stop falling till dash is complete

        }
        else
        {
            float x = movement.x;
            float z = movement.y;


            // creates vector 3 for moving player with character controller.
            // left/right, transform.right uses player facing direction, and moves it's right
            // forward/backward, transform.forward  using facing direction, moves forward
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            // out of dashing, decrement cooldown timer and disable dash when cooldown over
            if (isDashing && dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;

                if(dashCooldownTimer <= 0)
                {
                    isDashing = false;
                }
            }

        }
    }

    private void dash()
    {
        // If the player presses the dash key, initiates the dash
        if(inputDash == 1 && !isDashing)
        {   
            isDashing = true;

            // If the player is not pressing anything, dash forward
            if(movement == Vector2.zero)
            {
                dashDirection = Vector2.up;
            }
            // Assign dash direction to the direction the player is moving
            else
            {
                dashDirection = movement;
            }

            // Initiates timers
            dashCooldownTimer = dashCooldown;
            dashingTimer = dashLength;
        }
    }

    private void Jump()
    {
        // Jump mode chooser
        switch(jumpModeToggle)
        {
            case JumpModeToggle.BUNNY_HOP:
                // Bunnyhop Mode
                // TODO: Mimick Source/Quake 3 bunnyhopping:
                // https://adrianb.io/2015/02/14/bunnyhop.html
                // - Air strafing only gives speed (must strafe while turning with mouse to increase speed)
                // - Holding jump auto hops
                // - Player keeps moving forward while bunny hopping
                // - Pressing back causes speed to to go zero and can't reset easily
                if(jump == 1 && isGrounded)
                {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    // Bunnyhop?
                    if(movement.y > 0 || movement.x != 0)
                    {
                        speed += bunnyhopSpeedIncrementAmt;
                    }

                }
                if(jump == 0 && isGrounded)
                {
                    isJumping = false;
                    speed = defaultSpeed;
                }
                break;

            case JumpModeToggle.HIGH_JUMP:
                if(jump == 1 && isGrounded)
                {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
                if(jump == 1 && isJumping == true)
                {
                    if(jumpTimeCounter > 0)
                    {
                        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                        jumpTimeCounter -= Time.deltaTime;
                    }
                    else
                    {
                        isJumping = false;
                    }
                }
                if(jump == 0)
                {
                    isJumping = false;
                }
                break;


            case JumpModeToggle.DOUBLE_JUMP:
                if(jump == 1 && isGrounded)
                {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
                if(jump == 1 && isJumping == true)
                {
                    if(jumpTimeCounter > 0)
                    {
                        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                        jumpTimeCounter -= Time.deltaTime;
                    }
                    else
                    {
                        isJumping = false;
                    }
                }
                if(jump == 0)
                {
                    isJumping = false;
                }
                break;

        }

        // Gravity
        velocity.y += (gravity * fallGravityMultiplyer) * Time.deltaTime ;
        controller.Move(velocity * Time.deltaTime);
    }
}
