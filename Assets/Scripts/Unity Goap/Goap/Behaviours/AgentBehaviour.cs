using System.Collections.Generic;
using System.Linq;
using Goap;
using Goap.Classes;
using Goap.Classes.References;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;

namespace Goap.Behaviours
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        private IAgentMover mover;
        private BossAgent bossAgent;
        private WormAgent wormAgent;
        private GoapSetBehaviour goapSetBehaviour;

        public IAgentMover Mover => mover;
        
        public AgentState State { get; private set; } = AgentState.NoAction;

        private IGoapSet goapSet;
        public IGoapSet GoapSet
        {
            get => goapSet;
            set
            {
                goapSet = value;
                value.Register(this);
            }
        }

        private int indexGoal;
        public IGoalBase CurrentGoal { get; private set; }
        public IActionBase CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public float CurrentActionInRange { get; set; }
        public List<IActionBase> CurrentActionPath { get; private set; } = new List<IActionBase>();
        public IWorldData WorldData { get; } = new LocalWorldData();
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }

        private void Awake()
        {
            Injector = new DataReferenceInjector(this);
            mover = GetComponent<IAgentMover>();
            bossAgent = GetComponent<BossAgent>();
            wormAgent = GetComponent<WormAgent>();
            if (goapSetBehaviour != null)
                GoapSet = goapSetBehaviour.Set;
        }

        private void OnEnable()
        {
            Events.OnNoActionFound += SetAnotherGoal;
            Events.OnGoalCompleted += CheckIndexGoal;
            indexGoal = 0;
            if (GoapSet != null)
            {
                GoapSet.Register(this);
                GoapSet.SortGoal();
                DoToGoal();
            }
        }
        
        private void OnDisable()
        {
            if (GoapSet != null)
                GoapSet.Unregister(this);
        }

        public void Run()
        {
            if (CurrentAction == null)
            {
                State = AgentState.NoAction;
                return;
            }

            switch (CurrentAction.Config.MoveMode)
            {
                case ActionMoveMode.MoveBeforePerforming:
                    RunMoveBeforePerforming();
                    break;
                case ActionMoveMode.PerformWhileMoving:
                    RunPerformWhileMoving();
                    break;
                case ActionMoveMode.PerformNotMoving:
                    PerformNotMoving();
                    break;
            }
        }

        private void PerformNotMoving()
        {
            State = AgentState.PerformingAction;
            PerformAction();
        }
        private void RunPerformWhileMoving()
        {
            if (GetDistance() > CurrentActionInRange)
            {
                State = AgentState.MovingWhilePerformingAction;

                Move();
                PerformAction();
                return;
            }
            
            State = AgentState.PerformingAction;
            PerformAction();
        }

        private void RunMoveBeforePerforming()
        {
            if (GetDistance() <= CurrentActionInRange)
            {
                State = AgentState.PerformingAction;
                PerformAction();
                return;
            }

            State = AgentState.MovingToTarget;
            Move();
        }

        private void Move()
        {
            if (Mover == null)
                return;
            
            Mover.Move(CurrentActionData.Target);
        }

        private void PerformAction()
        {
            var result = CurrentAction.Perform(this, CurrentActionData, Time.deltaTime);

            if (result == ActionRunState.Continue)
                return;
            
            EndAction();
        }

        private float GetDistance()
        {
            
            if (CurrentActionData?.Target == null)
            {
                return 0f;
            }

            return Vector3.Distance(transform.position, CurrentActionData.Target.Position);
        }

        public void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase
        {
            SetGoal(GoapSet.ResolveGoal<TGoal>(), endAction);
        }

        public void DoToGoal()
        {
            if (CurrentGoal == null)
            {
                CurrentGoal = GoapSet.goals[indexGoal];
            }
            if (CurrentAction == null)
                GoapSet.Agents.Enqueue(this);
            
            Events.GoalStart(CurrentGoal);
        }

        public void SetGoal(IGoalBase goal, bool endAction)
        {
            if (goal == CurrentGoal)
                return;
            
            CurrentGoal = goal;
            
            if (CurrentAction == null)
                GoapSet.Agents.Enqueue(this);
            
            Events.GoalStart(goal);
            
            if (endAction)
                EndAction();
        }

        public void SetAction(IActionBase action, List<IActionBase> path, ITarget target)
        {
            if (CurrentAction != null)
            {
                EndAction(false);
            }

            CurrentAction = action;

            var data = action.GetData();
            Injector.Inject(data);
            CurrentActionData = data;
            CurrentActionData.Target = target;
            CurrentActionPath = path;
            CurrentAction.Start(this, CurrentActionData);
            CurrentActionInRange = CurrentAction.GetInRange(this, CurrentActionData);
            Events.ActionStart(action);
        }
        
        public void EndAction(bool enqueue = true)
        {
            var action = CurrentAction;
            
            CurrentAction?.End(this, CurrentActionData);
            CurrentAction = null;
            CurrentActionData = null;
            
            Events.ActionStop(action);

            if (enqueue)
               GoapSet.Agents.Enqueue(this);
        }

        public void SetAnotherGoal(IGoalBase goal)
        {
            indexGoal += 1;
            if (indexGoal >= GoapSet.goals.Count)
            {
                indexGoal = 0;
            }

            CurrentGoal = GoapSet.goals[indexGoal];
            if (CurrentAction == null)
                GoapSet.Agents.Enqueue(this);
            
            Events.GoalStart(CurrentGoal);
        }

        public void CheckIndexGoal(IGoalBase goal)
        {
            if (CurrentGoal is WanderGoal && bossAgent != null)
            {
                bossAgent.IsWandering = false;
            }

            if (CurrentGoal is KillPlayerGoal && bossAgent != null)
            {
                bossAgent.AttackDone = false;
            }

            if (CurrentGoal is LiveGoal)
            {
                if (bossAgent != null)
                {
                    bossAgent.UsedSkill = false;
                }
                if (wormAgent != null)
                {
                    wormAgent.DidAction = false;
                    CurrentGoal = GoapSet.goals[0];
                    if (CurrentAction == null)
                        GoapSet.Agents.Enqueue(this);
                    Events.GoalStart(CurrentGoal);
                }
            }
            if (indexGoal == 0)
            {
                return;
            }
            indexGoal = 0;
            CurrentGoal = GoapSet.goals[indexGoal];
            if (CurrentAction == null)
                GoapSet.Agents.Enqueue(this);
            Events.GoalStart(CurrentGoal);
        }
    }
}