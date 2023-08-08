using Cainos;
using Cainos.CustomizablePixelCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PixelPlayerController : MonoBehaviourPun, IPunObservable
{
    private PixelCharacter fx;

    PlayerUnit playerUnit;
    Rigidbody2D rb;
    CapsuleCollider2D col2D;

    Vector2 groundCheck;
    Vector2 wallCheck;
    float groundCheckRadius;
    float wallCheckRadius;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    //[SerializeField] GameObject SwordAttackVFX;
    [SerializeField] Collider2D WeaponCollider;

    public float horizontal;
    public bool isGrounded;
    public bool isTouchingWall;
    //flip
    //private bool isFacingRight = true;
    [SerializeField] public float faceDirection;

    //jump
    public int curJumpTimes;//so lan nhay
    public float jumpVelocity;

    //wall sliding
    public bool isWallSliding;
    protected float wallSlidingSpeed = 1f;

    //wall jum
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpVelocity = new Vector2(15, 20);

    //dash
    private float dashVelocity = 40f;
    private float dashDuration = 0.2f;
    public bool canDash;
    public bool isDash;

    //attack
    private int comboIndex;
    private bool canActtack;
    private float remainTimeToNextAttack;

    //photon
    PhotonView PV;
    private bool Photnet_AnimSyncAttack = false;

    private void Awake()
    {
        playerUnit = GetComponent<PlayerUnit>();
        rb = GetComponent<Rigidbody2D>();
        col2D = GetComponent<CapsuleCollider2D>();
        fx = GetComponent<PixelCharacter>();
        PV = GetComponent<PhotonView>();

    }
    // Start is called before the first frame update
    void Start()
    {
        //set stats
        //isFacingRight = transform.right.x > 0f;
        faceDirection = 1;

        canDash = true;
        isDash = false;
        comboIndex = 0;
        remainTimeToNextAttack = 0;
        canActtack = true;
        EnableWeaponCollider(false);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom && !PV.IsMine)
        {
            return;
        }
        horizontal = Input.GetAxis("Horizontal");
        if(isDash == false) fx.MovingBlend = Mathf.Abs(horizontal);

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
            //animator.SetTrigger(m_AnimDashId);
            if (faceDirection > 0)
            {
                rb.velocity = new Vector2(dashVelocity, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-dashVelocity, rb.velocity.y);
            }

            fx.MovingBlend = Mathf.Abs(dashVelocity);
            Invoke(nameof(ResetDash), dashDuration);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && curJumpTimes < playerUnit.curStat.jumpTimes)// khi cham dat, rest so lan nhay
        {
            curJumpTimes = playerUnit.curStat.jumpTimes;
        }

        if (Input.GetButtonDown("Jump") && CanJump())
        {
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

            //animator.SetTrigger(m_AnimJumpId);
        }
        fx.SpeedVertical = rb.velocity.y;
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
            CancelInvoke(nameof(StopWallJumping));
        }

        if (Input.GetButtonDown("Jump") && (isWallSliding || isTouchingWall) && isGrounded == false)
        {
            isWallJumping = true;

            wallJumpingDirection = -faceDirection;
            CancelInvoke(nameof(StopWallJumping));

            rb.velocity = new Vector2(wallJumpingDirection * wallJumpVelocity.x, wallJumpVelocity.y);

            faceDirection *= -1;
            fx.Facing = Mathf.RoundToInt(faceDirection);

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
        //animator.SetBool(m_AnimWallSlidingId, isWallSliding);
    }

    private void Flip()
    {
        //if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        //{
        //    isFacingRight = !isFacingRight;
        //    //transform.Rotate(0, 180, 0);
        //    fx.Facing = Mathf.RoundToInt(horizontal);
        //}

        if(faceDirection > 0 && horizontal < 0f || faceDirection < 0 && horizontal > 0f)
        {
            faceDirection *= -1;
            fx.Facing = Mathf.RoundToInt(faceDirection);
        }
    }

    protected bool CanJump()
    {
        return curJumpTimes > 0;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.InRoom && !PV.IsMine)
        {
            return;
        }
        if (isWallJumping == false && isDash == false)
        {
            rb.velocity = new Vector2(horizontal * playerUnit.curStat.movementVelocity, rb.velocity.y);
        }
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset

        groundCheck = (Vector2)transform.position + col2D.offset;
        wallCheck = (Vector2)transform.position + col2D.offset;
        groundCheckRadius = col2D.size.y / 2 + 0.1f;
        wallCheckRadius = col2D.size.x / 2 + 0.15f;
        isGrounded = Physics2D.Raycast(groundCheck, Vector2.down, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.Raycast(wallCheck, Vector2.right * faceDirection, wallCheckRadius, wallLayer);

        fx.IsGrounded = isGrounded;
    }

    private void HandleAttack()
    {
        if (remainTimeToNextAttack > 0 && canActtack == false)
        {
            remainTimeToNextAttack -= Time.deltaTime;
            if (remainTimeToNextAttack <= 0)
            {
                canActtack = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && canActtack)
        {
            CancelInvoke(nameof(RestCombo));
            EnableWeaponCollider(false);

            remainTimeToNextAttack = playerUnit.curStat.atkSpeed;
            canActtack = false;

            Photnet_AnimSyncAttack = true;
            //animator.SetTrigger(m_AnimAttackId);
            fx.Attack();
            fx.IsAttacking = true;
            comboIndex++;
            if (comboIndex > 4)
            {
                comboIndex = 1;
            }
            EnableWeaponCollider(true);
            Invoke(nameof(RestCombo), playerUnit.curStat.timeResetCombo);
        }
        //animator.SetInteger(m_AnimComboIndexId, comboIndex);
    }

    private void EnableWeaponCollider(bool isEnable)
    {
        if(WeaponCollider != null)
        {
            WeaponCollider.enabled = isEnable;
        }
    }

    private void RestCombo()
    {
        comboIndex = 0;
        fx.IsAttacking = false;
        EnableWeaponCollider(false);
    }

    private void OnDrawGizmosSelected()
    {
        //Draw the ground detection circle
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck, groundCheckRadius);//ground
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallCheck, wallCheckRadius);//wall
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(fx.Facing);
            stream.SendNext(fx.IsAttacking);
            stream.SendNext(fx.IsGrounded);
            stream.SendNext(fx.MovingBlend);
            stream.SendNext(fx.SpeedVertical);

            stream.SendNext(Photnet_AnimSyncAttack);
            Photnet_AnimSyncAttack = false;
        }
        else if(stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

            fx.Facing = (int)stream.ReceiveNext();
            fx.IsAttacking = (bool)stream.ReceiveNext();
            fx.IsGrounded = (bool)stream.ReceiveNext();
            fx.MovingBlend = (float)stream.ReceiveNext();
            fx.SpeedVertical = (float)stream.ReceiveNext();
            Photnet_AnimSyncAttack = (bool)stream.ReceiveNext();
            if (Photnet_AnimSyncAttack)
            {
                fx.Attack();
                Photnet_AnimSyncAttack = false;
            }
        }
    }
}
