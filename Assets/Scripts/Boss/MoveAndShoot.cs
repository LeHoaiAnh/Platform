using Photon.Pun;
using System;

using UnityEngine;
using Random = UnityEngine.Random;

public class MoveAndShoot : MonoBehaviourPun, IPunObservable
{
    private BossController bossController;
    [SerializeField] private float maxRangeMove;
    private float leftEdge;
    private float rightEdge;
    private Vector3 curTarget;

    private bool Photnet_AnimSyncAttack = false;
    private void Awake()
    {
        bossController = GetComponent<BossController>();
    }

    private void OnEnable()
    {
        leftEdge = transform.position.x - maxRangeMove;
        rightEdge = transform.position.x + maxRangeMove;
        FindMovePos();
    }

    private void FindMovePos()
    {
        float posX = transform.position.x + Random.Range(-2f, 2f);
        posX = posX < leftEdge ? leftEdge : posX;
        posX = posX > rightEdge ? rightEdge : posX;
        curTarget = new Vector3(posX, transform.position.y, transform.position.z);
        RotataTransform(curTarget);
    }

    public void RotataTransform(Vector3 target)
    {
        if (target.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0 ,0 , 0);
        }
        else if (target.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0 ,180 , 0);
        }
    }
    
    private void Move()
    {
        if (Mathf.Abs(transform.position.x - curTarget.x) <= 0.1f)
        {
            FindMovePos();
        }
        else
        {
            transform.position += (curTarget - transform.position).normalized * bossController.currentStats.speed *
                                  Time.deltaTime;
            
        }
    }
    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient == false) return;
        }
        bool shoot = false;
        if (bossController.IsAlive() && bossController.CheckTargetInRange())
        {
            shoot = bossController.shooter.FireAt(bossController.target.transform.position, bossController.target);
        }
        else
        {
            bossController.target = null;
        }

        if (!shoot)
        {
            Move();
        }
        else
        {
            bossController.animator.SetTrigger(bossController.mAnimAttack);
            Photnet_AnimSyncAttack = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Photnet_AnimSyncAttack);
            Photnet_AnimSyncAttack = false;
        }
        else if (stream.IsReading)
        {
            Photnet_AnimSyncAttack = (bool)stream.ReceiveNext();
            if (Photnet_AnimSyncAttack)
            {
                bossController.animator.SetTrigger(bossController.mAnimAttack);
                Photnet_AnimSyncAttack = false;
            }
        }
    }
}
