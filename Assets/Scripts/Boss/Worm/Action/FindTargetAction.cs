using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;

public class FindTargetAction : ActionBase<FindTargetAction.Data>
{
    private WormAgent worm;
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
    }

    public override void Created()
    {
    }
    
    public override void Start(IMonoAgent agent, Data data)
    {
        worm = agent.GetComponent<WormAgent>();
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        FindTarget();
        return ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, Data data)
    {
        
    }

    public void FindTarget()
    {
        //if (QuanLyManChoi.Instance != null && QuanLyManChoi.Instance.gameState == GAMESTATE.INGAME && PlayerManagement.Instance.PlayerUnit.IsAlive())
        //{
        //    worm.Target = PlayerManagement.Instance.PlayerUnit.transform;
        //}
        //else
        //{
        //    worm.Target = null;
        //}
        if(worm.bossController.target != null)
        {
            worm.Target = worm.bossController.target.transform;
        }
        else
        {
            worm.Target = null;
        }
    }
        
}
