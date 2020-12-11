using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    public Equipment.EquipmentType equipType;
    public Weapon.WeaponType weaponType;

    public override void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();

                Item itemTemp = transform.GetChild(0).GetComponent<ItemUI>().Item;
                //脱掉放到背包里面
                DestroyImmediate(currentItemUI.gameObject);
                transform.parent.SendMessage("PutOff", itemTemp);
                InventoryManager.Instance.HideToolTip();
            }
        }
        //手上有东西
        //当前装备槽 有装备
        //无装备
        //手上没东西
        //当前装备槽 有装备
        //无装备 不做处理

        bool isUpdateProperty = false;
        if (InventoryManager.Instance.IsPickedItem == true)
        {

            //手上有东西的情况
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>(); //当前装备的槽里面的物品                                                      
                if (IsRightItem(pickedItem.Item))
                {
                    InventoryManager.Instance.PickedItem.ExChange(currentItemUI);
                    isUpdateProperty = true;
                }
            }
            else
            {
                if (IsRightItem(pickedItem.Item))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem(1);
                    isUpdateProperty = true;
                }
            }
        }
        else
        {
            //手上没装备的情况
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>(); //获取当前装备槽里面的物品
                InventoryManager.Instance.PickUpItem(currentItemUI.Item, currentItemUI.Amount); //将装备槽里面的物品信息放到手上
                Destroy(currentItemUI.gameObject);
                isUpdateProperty = true;
            }

        }
        if (isUpdateProperty)
        {
            transform.SendMessage("UpdateProtertyText");
        }

    }

    //判断item是否适合放在这个位置
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)(item)).EquipmentTypes == this.equipType) ||
            (item is Weapon    && ((Weapon)(item)).WeaponTypes       == this.weaponType)){
            return true;
        }
        return false;
    }

}
