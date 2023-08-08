using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListShopItem : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField]private TypeItem type;
    [SerializeField] private ShopItem itemPrefab;
    
    public void SetGroupItem(NodePagination nodePagination)
    {
        ItemTypeCfg itemTypeCfg = ConfigManager.itemCfg.GetListItemByType(type);
        container.DestroyChildren();

        if (itemTypeCfg != null)
        {
            nodePagination.SetItem(type);
            foreach (var VARIABLE in itemTypeCfg.items)
            {
                var obj = SimplePool.Spawn(itemPrefab.gameObject, container);
                var objItem = obj.GetComponent<ShopItem>();
                objItem.SetStats(itemTypeCfg.typeItem, VARIABLE);
            }
        }
    }
}
