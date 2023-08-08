﻿using UnityEngine.UIElements;

namespace Goap.Editor.Elements
{
    public class Header : VisualElement
    {
        public Header(string text)
        {
            this.name = "header";
            this.Add(new Label(text));
        }
    }
}