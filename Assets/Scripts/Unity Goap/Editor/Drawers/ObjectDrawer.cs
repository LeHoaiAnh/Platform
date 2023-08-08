using Goap.Classes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Goap.Editor.Drawers
{
    public class ObjectDrawer : VisualElement
    {
        public ObjectDrawer(object obj)
        {
            if (obj is null)
                return;
            
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                
                this.Add(new Label($"{property.Name}: {this.GetValueString(value)}"));
            }
        }

        private string GetValueString(object value)
        {
            if (value is null)
                return "null";
            
            if (value is TransformTarget transformTarget)
            {
                if (transformTarget.Transform != null)
                {
                    return transformTarget.Transform.name;
                }
                else
                {
                    return "null";
                }
            }
            if (value is PositionTarget positionTarget)
                return positionTarget.Position.ToString();
            
            if (value is MonoBehaviour monoBehaviour)
                return monoBehaviour.name;
            
            if (value is ScriptableObject scriptableObject)
                return scriptableObject.name;

            return value.ToString();
        }
    }
}