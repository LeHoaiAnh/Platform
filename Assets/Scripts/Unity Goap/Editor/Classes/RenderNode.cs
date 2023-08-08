﻿using Goap.Editor.Classes.Models;
using UnityEngine;

namespace Goap.Editor.Classes
{
    public class RenderNode
    {
        private readonly Nodes nodes;
        public Node Node { get; }
        
        public Vector2 Position { get; set; }
        public Rect Rect => new Rect(this.Position.x, this.Position.y, 200, 150);
        public int Depth { get; set; }

        public RenderNode(Node node, int depth)
        {
            this.Node = node;
            Depth = depth;
        }
    }
}