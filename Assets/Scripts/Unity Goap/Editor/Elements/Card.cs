using System;
using UnityEngine.UIElements;

namespace Goap.Editor.Elements
{
    public class Card : VisualElement
    {
        public Card(Action<Card> callback)
        {
            name = "card";
            callback(this);
        }
    }
}