using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
        /// Tween targets will be color-faded when drag events occure.
        /// </summary>
        public List<Image> TweenTargets;

        /// <summary>
        /// If drop is allowed, the spot will be faded with this color.
        /// </summary>
        public Color ColorDropAllowed = new Color(0.5f, 1f, 0.5f);

        /// <summary>
        /// If drop is not allowed, the spot will be faded with this color.
        /// </summary>
        public Color ColorDropDenied = new Color(1f, 0.5f, 0.5f);
        
        /// <summary>
        /// Becomes [true] when drag was started and mouse position is over this drag receiver.
        /// </summary>
        public static bool DropReady;

        private InventoryItemSlot itemSlot;

        private void Start()
        {
            itemSlot = GetComponentInParent<InventoryItemSlot>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryItemUI.DragTarget == null) return;
            if (itemSlot.type == InventoryItemUI.DragTarget.inventoryInfor.inventoryInfor.typeItem)
            {
                Fade(ColorDropAllowed);
                DropReady = true;
            }
            else
            {
                Fade(ColorDropDenied);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DropReady = false;
            Fade(Color.white);
        }

        private void Fade(Color color)
        {
            TweenTargets.ForEach(i => i.CrossFadeColor(color, 0.25f, true, false));
        }
}
