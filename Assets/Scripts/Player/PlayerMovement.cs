using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerUnit playerUnit;
    Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] GameObject SwordAttackVFX;

    public float horizontal;
    public bool isGrounded;
    public bool isTouchingWall;
    //flip
    private bool isFacingRight = true;
    //jump
    public int curJumpTimes;//so lan nhay
    public float jumpVelocity;

    //wall sliding
    public bool isWallSliding;
    protected float wallSlidingSpeed = 0f;

    //wall jum
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpVelocity = new Vector2(15, 20);

    // anim
    public Animator animator;
    public int m_AnimRunId;
    public int m_AnimGroundId;
    public int m_AnimJumpId;
    public int m_AnimWallSlidingId;
    public int m_AnimDashId;
    public int m_AnimAttackId;
    public int m_AnimComboIndexId;
    //dash
    private float dashVelocity = 40f;
    private float dashDuration = 0.2f;
    public bool canDash;
    public bool isDash;

    //attack
    private int comboIndex;
    private bool canActtack;
    private float remainTimeToNextAttack;
    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GetComponent<PlayerUnit>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_AnimRunId = Animator.StringToHash("run");
        m_AnimJumpId = Animator.StringToHash("jump");
        m_AnimGroundId = Animator.StringToHash("grounded");
        m_AnimWallSlidingId = Animator.StringToHash("wallSliding");
        m_AnimDashId = Animator.StringToHash("dash");
        m_AnimAttackId = Animator.StringToHash("attack");
        m_AnimComboIndexId = Animator.StringToHash("comboIndex");

        //set stats
        isFacingRight = transform.right.x > 0f;
        canDash = true;
        isDash = false;
        comboIndex = 0;
        remainTimeToNextAttack = 0;
        canActtack = true;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        animator.SetBool(m_AnimRunId, horizontal != 0);

        GroundedCheck();
        HandleDash();
        HandleJump();
        WallSlide();
        WallJump();

        if (isWallJumping == false && isDash == false)
        {
            Flip();
        }

        HandleAttack();
    }
    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.K) && canDash && isWallSliding == false)
        {
            isDash = true;
            canDash = false;
            animator.SetTrigger(m_AnimDashId);
            if (isFacingRight)
            {
                rb.velocity = new Vector2(dashVelocity, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-dashVelocity, rb.velocity.y);
            }

            Invoke(nameof(ResetDash), dashDuration);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && curJumpTimes < playerUnit.curStat.jumpTimes)// khi cham dat, rest so lan nhay
        {
            curJumpTimes = playerUnit.curStat.jumpTimes;
            Debug.Log("reset jump");
        }

        if (Input.GetButtonDown("Jump") && CanJump())
        {
            animator.SetTrigger(m_AnimJumpId);
            if (curJumpTimes < playerUnit.curStat.jumpTimes)
            {
                jumpVelocity = playerUnit.curStat.jumpVelocity * playerUnit.curStat.inAirJumpMultiplier;
            }
            else
            {
                jumpVelocity = playerUnit.curStat.jumpVelocity;
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            curJumpTimes -= 1;
        }
    }
    private void ResetDash()
    {
        canDash = true;
        isDash = false;
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.right.x;

            CancelInvoke(nameof(StopWallJumping));
        }

        if (Input.GetButtonDown("Jump") && (isWallSliding || isTouchingWall) && isGrounded == false)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpVelocity.x, wallJumpVelocity.y);

            if (transform.right.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180f, 0);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void WallSlide()
    {
        if (isTouchingWall && isGrounded == false && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
        animator.SetBool(m_AnimWallSlidingId, isWallSliding);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }
   
    protected bool CanJump()
    {
        return curJumpTimes > 0;
    }

    private void FixedUpdate()
    {
        if (isWallJumping == false && isDash == false)
        {
            rb.velocity = new Vector2(horizontal * playerUnit.curStat.movementVelocity, rb.velocity.y);
        }   
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector3.down, 0.09f, groundLayer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.3f, wallLayer);

        animator.SetBool(m_AnimGroundId, isGrounded);
    }

    private void HandleAttack()
    {
        if(remainTimeToNextAttack > 0 && canActtack == false)
        {
            remainTimeToNextAttack -= Time.deltaTime;
            if(remainTimeToNextAttack <= 0)
            {
                canActtack = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && canActtack)
        {
            CancelInvoke(nameof(RestCombo));

            remainTimeToNextAttack = playerUnit.curStat.atkSpeed;
            canActtack = false;

            animator.SetTrigger(m_AnimAttackId);
            comboIndex++;
            if(comboIndex > 4)
            {
                comboIndex = 1;
            }
            Invoke(nameof(RestCombo), playerUnit.curStat.timeResetCombo);
        }
        animator.SetInteger(m_AnimComboIndexId, comboIndex);
    }

    private void RestCombo()
    {
        comboIndex = 0;
    }
    public void EnableSwordAttackVFX()
    {
        SwordAttackVFX.SetActive(true);
    }

    public void AnimationFinished()
    {
        SwordAttackVFX.SetActive(false);
    }

    public void PlayAttackSound()
    {

    }
}
