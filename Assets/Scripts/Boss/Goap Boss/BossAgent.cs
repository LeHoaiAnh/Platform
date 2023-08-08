using System;
using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAgent : MonoBehaviour
{
    [SerializeField] private BossDynamicLightController light;
    
    private AgentBehaviour agent;
    private IGoapRunner goapRunner;
    private BossController bossController;
    public bool IsWandering { get; set; }
    public bool AttackDone { get; set; }
    public bool UsedSkill { get; set; }
    private void Awake()
    {
        agent = GetComponent<AgentBehaviour>();
        goapRunner = GetComponentInChildren<GoapRunnerBehaviour>();
        bossController = GetComponent<BossController>();
    }

    private void Start()
    {
        agent.GoapSet = goapRunner.GetSet("Boss");
        agent.GoapSet.SortGoal();
        agent.DoToGoal();
        InitForMove();
    }
    
    
    [SerializeField] private float range = 10;
    private Vector3 startPos;  
    private Vector3 leftEdgePos;  
    private Vector3 rightEdgePos;
    
    
    void InitForMove()
    {
        startPos = transform.position;
        leftEdgePos = startPos + Vector3.left * range;
        rightEdgePos = startPos + Vector3.right * range;
    }

    public Vector3 GetTargetWander()
    {
        Vector3 newTarget;
        if (Vector3.Distance( transform.position , leftEdgePos) <= 0.01f)
        {
            newTarget = rightEdgePos;
        }
        else  if (Vector3.Distance( transform.position , leftEdgePos) <= 0.01f)
        {
            newTarget = leftEdgePos;
        }
        else if (transform.position.x < startPos.x)
        {
            newTarget = leftEdgePos;
        }
        else
        {
            newTarget = rightEdgePos;
        }

        return Vector3.MoveTowards(transform.position, new Vector3(newTarget.x, transform.position.y, newTarget.z), Time.deltaTime * bossController.currentStats.speed);
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
    public bool CanSeePlayer()
    {
        return Mathf.Abs(transform.position.x - bossController.target.transform.position.x) <= bossController.currentStats.seeRange 
               && Mathf.Abs(transform.position.y - bossController.target.transform.position.y) <= 1.5f;
    }

    public bool IsLowHP()
    {
        return bossController.currentStats.HP <= 0.2f * bossController.initStats.HP;
    }

    public void ActiveSkill()
    {
        if (light != null)
        {
            light.UpdateColor(TypeLight.DAMAGING_LIGHT);
        }
    }
    
    public void CancleSkill()
    {
        if (light != null)
        {
            light.UpdateColor(TypeLight.NEUTRAL_LIGHT);
        }
    }
    
}
