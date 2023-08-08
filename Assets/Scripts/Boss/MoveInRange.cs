using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveInRange : MonoBehaviourPun
{
    private BossController bossController;
    [SerializeField] private float range = 5;
    
    [Header("MoveRound")]
    [SerializeField] private bool moveRound = false;
    [SerializeField] private float groundCheckDistance;
    [SerializeField]private float width;
    [SerializeField]private float height;
    
    private Vector3 startPos;  
    private Vector3 leftEdgePos;  
    private Vector3 rightEdgePos;
    private Vector3 curDir;
    private Vector3 curTarget;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
    }

    private void Start()
    {
        InitForMove();
    }

    void InitForMove()
    {
        startPos = transform.position;
        leftEdgePos = startPos + Vector3.left * range;
        rightEdgePos = startPos + Vector3.right * range;
        
        if (moveRound)
        {
            SetCurDirRound(Vector3.right);
        }
        else
        {
            if (Random.Range(0, 1) == 0)
            {
                SetCurDir(Vector3.left);
            }
            else
            {
                SetCurDir(Vector3.right);
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient == false) return;
        }
        if (bossController.IsAlive())
        {
            if (moveRound)
            {
                MoveRound();
            }
            else
            {
                Move();
            }
        }
    }

    void Move()
    {
        if (transform.position.x < leftEdgePos.x)
        {
            SetCurDir(Vector3.right);
        }
        else if ( transform.position.x > rightEdgePos.x )
        {
           SetCurDir(Vector3.left);
        }

        transform.position += curDir * bossController.currentStats.speed * Time.deltaTime;
    }

    void MoveRound()
    {
        bool isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckDistance, LayersMan.LayerGround);
        if (isGrounded)
        {
            transform.position += curDir * bossController.currentStats.speed * Time.deltaTime;
        }
        else
        {
            if (curDir == Vector3.right)
            {
                transform.position += new Vector3(0 -width/2, 0);
                SetCurDirRound(Vector3.down);
            }
            else if (curDir == Vector3.down)
            {
                transform.position += new Vector3(-width/2, 0, 0);
                SetCurDirRound(Vector3.left);
            }
            else if (curDir == Vector3.left)
            {
                transform.position += new Vector3(0, width/2, 0);
                SetCurDirRound(Vector3.up);
            }
            else if (curDir == Vector3.up)
            {
                transform.position += new Vector3(width/2, 0, 0);
                SetCurDirRound(Vector3.right);
            }
        }
    }

    void SetCurDir(Vector3 dir)
    {
        curDir = dir;
        if (curDir == Vector3.left)
        {
            transform.rotation = Quaternion.Euler(0 ,180, 0);
        }
        else if (curDir == Vector3.right)
        {
            transform.rotation = Quaternion.Euler(0 ,0, 0);
        }
    }
    
    void SetCurDirRound(Vector3 dir)
    {
        curDir = dir;
        if (curDir == Vector3.left)
        {
            transform.rotation = Quaternion.Euler(0 ,0, 180);
        }
        else if (curDir == Vector3.right)
        {
            transform.rotation = Quaternion.Euler(0 ,0, 0);
        }
        else if (curDir == Vector3.down)
        {
            transform.rotation = Quaternion.Euler(0 ,0, -90);
        }
        else if (curDir == Vector3.up)
        {
            transform.rotation = Quaternion.Euler(0 ,0, 90);
        }
    }
}
