using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInGround : MonoBehaviourPun, IPunObservable
{
    private BossController bossController;

    [SerializeField] private Transform groundDetectionDown;
    [SerializeField] private Transform groundDetectionUp;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField] private float ground_distance = 0.01f;
    [SerializeField] float appearDistance;
    [SerializeField] float godownDistance;
    
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private bool aboveGround;
    private void Awake()
    {
        bossController = GetComponent<BossController>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient == false) return;
        }
        if (bossController.IsAlive())
        {
            HideAndAppear();
            Flip();
        }
    }

    void HideAndAppear()
    {
        if (bossController.target == null)
        {
            bossController.SetDefaultTarget();
        }
        else
        {
            float disToTarget = Vector2.Distance(transform.position, bossController.target.transform.position);

            if (disToTarget < appearDistance && disToTarget > godownDistance)
            {
                aboveGround = true;
                Appear();
            }

            if (disToTarget > appearDistance || disToTarget < godownDistance)
            {
                aboveGround = false;
                Hide();
            }
        }
    }

    void Hide()
    {
        if (collider.enabled)
        {
            collider.enabled = false;
        }

        if (light.enabled)
        {
            light.enabled = false;
        }
        
        RaycastHit2D groundInfo_up = Physics2D.Raycast(groundDetectionUp.position, Vector2.up, ground_distance, LayersMan.LayerGround);
        if(groundInfo_up.collider == false)
        {
            transform.position += - transform.up * Time.deltaTime * bossController.currentStats.speed;
        }
    }

    void Appear()
    {
        if (collider.enabled == false)
        {
            collider.enabled = true;
        }

        if (light.enabled == false)
        {
            light.enabled = true;
        }
        
        RaycastHit2D groundInfo_down = Physics2D.Raycast(groundDetectionDown.position, Vector2.down, ground_distance, LayersMan.LayerGround);
        if(groundInfo_down.collider == true)
        {
            transform.position += transform.up * Time.deltaTime * bossController.currentStats.speed;
        }
    }
    
    void Flip()
    {
        if (bossController.target == null) return;

        if(bossController.target.transform.position.x > transform.position.x)
        {
            if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }
        if(bossController.target.transform.position.x < transform.position.x)
        {
            if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    
    void CreateBullets()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient == false)
        {
            return;
        }
        if (bossController.shooter && aboveGround && bossController.target != null)
        {
            Vector3 dir = transform.position.x < bossController.target.transform.position.x ? Vector3.right : Vector3.left;
            bossController.shooter.FireAt(transform.position +  dir * 10f, bossController.target);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(spriteRenderer.flipX);
            stream.SendNext(aboveGround);
            stream.SendNext(collider.enabled);
            stream.SendNext(light.enabled);
            
        }
        else if (stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

            spriteRenderer.flipX = (bool)stream.ReceiveNext();
            aboveGround = (bool)stream.ReceiveNext();
            collider.enabled = (bool)stream.ReceiveNext();
            light.enabled = (bool)stream.ReceiveNext();
        }
    }
}
