using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Weapon")]
    [SerializeField] Weapon _weapon;

    [SerializeField] PlayerItemCollectorHandler _itemCollectHandler;
    [SerializeField] PlayerInteractionHandler _interractionHandler;

    [SerializeField] Rigidbody2D _rb;

    [SerializeField] float speed = 2;
    [SerializeField] float jumpPower = 5;
    [SerializeField] float slideFactor = 0.2f;
    [SerializeField] int totalJumps;
    const float groundCheckRadius = 0.2f;

    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;


    int availableJumps;
    float horizontalValue;
    float runSpeedModifier = 2f;
    
    bool isGrounded = true;    
    bool isRunning;
    bool facingRight = true;
    bool multipleJump;
    bool coyoteJump;




    void Start()
    {
        availableJumps = totalJumps;
        _interractionHandler.Reset();
        _weapon.SetGunStats();
    }

    void Update()
    {


        if (Input.GetMouseButton(0))
        {
            _weapon.StartShootingContinuesly();
        } 
        
        else if (Input.GetMouseButtonUp(0))
        {
            _weapon.StopShootingContinuesly();
        }






        //Store the horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");


        //If LShift is clicked enable isRunning
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        //If LShift is released disable isRunning
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;


        //If we press Jump button enable jump 
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y / 2);
        }

        
        // Try to interract
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interractionHandler.TryToInteract();
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue);        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        
        //Check if the GroundCheckObject is colliding with other
        //2D Colliders that are in the "Ground" Layer
        //If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            groundCheckCollider.position, 
            groundCheckRadius, 
            groundLayer);


        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }        
        }    
        else
        {
            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }
    }




    #region Jump
    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

    void Jump()
    {
        if (isGrounded)
        {
            multipleJump = true;
            availableJumps--;

            _rb.velocity = Vector2.up * jumpPower;
        }
        else
        {
            if(coyoteJump)
            {
                multipleJump = true;
                availableJumps--;

                _rb.velocity = Vector2.up * jumpPower;
            }

            if(multipleJump && availableJumps>0)
            {
                availableJumps--;

                _rb.velocity = Vector2.up * jumpPower;
            }
        }
    }
    #endregion






    void Move(float dir)
    {

        #region Move & Run
        //Set value of x using dir and speed
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        //If we are running mulitply with the running modifier (higher)
        if (isRunning)
            xVal *= runSpeedModifier;
        //Create Vec2 for the velocity
        Vector2 targetVelocity = new Vector2(xVal, _rb.velocity.y);
        //Set the player's velocity
        _rb.velocity = targetVelocity;
 
        //If looking right and clicked left (flip to the left)
        if(facingRight && dir < 0)
        {
            facingRight = false;
        }
        //If looking left and clicked right (flip to rhe right)
        else if(!facingRight && dir > 0)
        {
            facingRight = true;
        }

        //(0 idle , 4 walking , 8 running)
        //Set the float xVelocity according to the x value 
        //of the RigidBody2D velocity 
        #endregion
    } 











    // Collision logic
    public void OnTriggerEnter2D(Collider2D col)
    {
        // Collectible item logic
        if (col.tag == TagConstraintsConfig.COLLECTIBLE_ITEM_TAG)
        {
            BasicDropItem itemData = col.gameObject.GetComponent<BasicDropItem>();
            if (itemData != null)
            {
                _itemCollectHandler.CollectItem(itemData);
            }
        }
    

        // // Effect zone logic
        // if (col.tag == TagConstraintsConfig.EFFECT_ZONE_TAG)
        // {
        //     BasicEffectZone environment = col.gameObject.GetComponent<BasicEffectZone>();
        //     if (environment != null)
        //     {
        //         _effectsHandler.ApplyEffect(environment.EffectData);
        //     }
        // }


        // Interactible zone logic
        if (col.tag == TagConstraintsConfig.INTERACTIBLE_ZONE_TAG)
        {
            IInteractible data = col.gameObject.GetComponent<IInteractible>();
            if (data != null)
            {
                _interractionHandler.RegisterInteractible(data);                
            }
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        // if (col.tag == TagConstraintsConfig.EFFECT_ZONE_TAG)
        // {
        //     BasicEffectZone environment = col.gameObject.GetComponent<BasicEffectZone>();
        //     if (environment != null)
        //     {
        //         _effectsHandler.RemoveEffect(environment.EffectData);
        //     }
        // }


        // Interactible zone logic
        if (col.tag == TagConstraintsConfig.INTERACTIBLE_ZONE_TAG)
        {
            IInteractible data = col.gameObject.GetComponent<IInteractible>();
            if (data != null)
            {
                _interractionHandler.UnregisterInteractible(data);                
            }
        }
    }











    
    void WallCheck()
    {
        //If we are touching a wall
        //and we are moving towards the wall
        //and we are falling
        //and we are not grounded
        //Slide on the wall
        // if (Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer)
        //     && Mathf.Abs(horizontalValue) > 0
        //     && rb.velocity.y < 0
        //     && !isGrounded)
        // {
        //     if(!isSliding)
        //     {
        //         availableJumps = totalJumps;
        //         multipleJump = false;
        //     }

        //     Vector2 v = rb.velocity;
        //     v.y = -slideFactor;
        //     rb.velocity = v;
        //     isSliding = true;

        //     if(Input.GetButtonDown("Jump"))
        //     {
        //         availableJumps--;

        //         rb.velocity = Vector2.up * jumpPower;
        //         animator.SetBool("Jump", true);
        //     }
        // }
        // else
        // {
        //     isSliding = false;
        // }
    }
}