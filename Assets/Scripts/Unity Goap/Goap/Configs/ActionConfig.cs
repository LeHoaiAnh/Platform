﻿using Goap.Configs.Interfaces;
using Goap.Enums;
using Goap.Interfaces;

namespace Goap.Configs
{
    public class ActionConfig : IActionConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public int BaseCost { get; set; }
        public ITargetKey Target { get; set; }
        public float InRange { get; set; }
        public ICondition[] Conditions { get; set; }
        public IEffect[] Effects { get; set; }
        public ActionMoveMode MoveMode { get; set; } = ActionMoveMode.MoveBeforePerforming;
    }
}