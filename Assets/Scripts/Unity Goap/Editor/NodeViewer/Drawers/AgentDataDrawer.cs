using Goap.Editor.Drawers;
using Goap.Editor.Elements;
using Goap.Interfaces;
using UnityEngine.UIElements;

namespace Goap.Editor.NodeViewer.Drawers
{
    public class AgentDataDrawer : VisualElement
    {
        public AgentDataDrawer(IAgent agent)
        {
            this.name = "agent-data";
            
            var card = new Card((card) =>
            {
                card.Add(new Header("Agent Data"));
                card.Add(new ObjectDrawer(agent.CurrentActionData));
            });
            
            this.Add(card);
        }
    }
}