using System;
using System.Collections.Generic;

namespace Goap.Editor.Classes.Models
{
    public class NodeCondition
    {
        public string Condition { get; set; }
        
        public Guid[] Connections { get; set; } = {};
    }
}