using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler,  IEndDragHandler
{
    [SerializeField]private ShowIconItem Icon;
    [SerializeField]private Image Frame;
    [SerializeField]private TextMeshProUGUI Count;
    [SerializeField] private GameObject equiped;
    
    public InventoryInfor inventoryInfor { get; set; }
    public ItemStatsInfor statsInfor{ get; set; }
    private GameObject _phantom;
    private RectTransform _rect;
    public static InventoryItemUI DragTarget;
    public static Action<InventoryItemUI> OnDragCompleted;

    public void SetStas(InventoryInfor item)
    {
        inventoryInfor = InventoryInfor.Clone(item);
        statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(item.inventoryInfor.typeItem, item.inventoryInfor.codename);
        Icon.ShowImgIcon(item.inventoryInfor.typeItem, GameClient.instance.spriteCollection.GetSprite(item.inventoryInfor.typeItem + "_" + item.inventoryInfor.codename));
        Frame.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + statsInfor.Rarity);
        Count.text = item.count.ToString();
        equiped.SetActive(inventoryInfor.equiped);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PopupInventory.Instance != null)
        {
            PopupInventory.Instance.ShowInforItem(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);

        _phantom = Instantiate(gameObject);
        _phantom.transform.SetParent(canvas.transform, true);
        _phantom.transform.SetAsLastSibling();
        _phantom.transform.localScale = transform.localScale;
        _phantom.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
        _phantom.GetComponent<InventoryItemUI>().Count.text = "1";
        _rect = canvas.GetComponent<RectTransform>();
        SetDraggedPosition(eventData);
        DragTarget = this;
    }
    
    private void SetDraggedPosition(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rect, data.position, data.pressEventCamera, out var mouse))
        {
            var rect = _phantom.GetComponent<RectTransform>();

            rect.position = mouse;
            rect.rotation = _rect.rotation;
        }
    }
    private static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;

        var comp = go.GetComponent<T>();

        if (comp != null) return comp;

        var t = go.transform.parent;

        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }

        return comp;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (DragTarget == null) return;
        SetDraggedPosition(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragTarget == null) return;
        
        
        if (ItemDragReceiver.DropReady)
        {
            OnDragCompleted?.Invoke(this);
        }

        DragTarget = null;
        ItemDragReceiver.DropReady = false;
        Destroy(_phantom, 0.25f);

        foreach (var graphic in _phantom.GetComponentsInChildren<Graphic>())
        {
            graphic.CrossFadeAlpha(0f, 0.25f, true);
        }
    }
}
