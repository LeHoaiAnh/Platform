using System;
using System.Collections;
using System.Collections.Generic;
using Goap.Interfaces;
using UnityEngine;

public class AgentMoveBehaviour : MonoBehaviour, IAgentMover
{
    private ITarget target;
    private BossController bossController;
    private BossAgent bossAgent;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
        bossAgent = GetComponent<BossAgent>();
    }

    public void Move(ITarget target)
    {
        this.target = target;
            
        if (this.target == null)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(this.target.Position.x, transform.position.y, this.target.Position.z), Time.deltaTime * bossController.currentStats.speed);
        bossAgent.RotataTransform(target.Position);
    }

    private void OnDrawGizmos()
    {
        if (target == null)
            return;
            
        Gizmos.DrawLine(transform.position, target.Position);
    }
}