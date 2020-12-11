using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    public GameObject itemPrefab;
    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面已经有item了，amount++
    /// 如果没有 根据itemPrefab去实例化一个item，放在下面
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {

            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);


        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }

    /// <summary>
    /// 得到当前物品槽储存的物品类型
    /// </summary>
    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }
    /// <summary>
    /// 得到物品的ID
    /// </summary>
    /// <returns></returns>
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity; // 当前的数量大于等于容量
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTip();
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                //Debug.Log("111111111=" + transform.GetChild(0).GetComponent<ItemUI>());
                //Debug.Log("222222222=" + transform.GetComponent<ItemUI>());
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if(currentItemUI.Item is Equipment || currentItemUI.Item is Weapon)
                {
                 
                  
                    Item currentItem = currentItemUI.Item;
                
                    currentItemUI.ReducAmount(1);
                   
                    if (currentItemUI.Amount <= 0)
                    {
                        DestroyImmediate(currentItemUI.gameObject);
                        InventoryManager.Instance.HideToolTip();
                    }
                    CharacterPanel.Instance.PutOn(currentItem);

                }
            }
        }

        if(eventData.button != PointerEventData.InputButton.Left) return;
        //自身是空  
                   //1. IsPickedItem = true       pickedItem放在这个位置
                                // 按下ctrl      放置当前鼠标上的物品的一个
                                // 没有按下ctrl  放置当前鼠标上的所有物品
                   //2. IsPickedItem == false 不做任何处理
                   
        //自身不为空
                   //1. IsPickedItem == true
                         //自身的id == pickedItem.id
                                    // 按下ctrl           放置当前鼠标的物品的一个
                                    // 没有按下ctrl        放置当前鼠标上的物品的所有
                                                            //完全可以放下
                                                            //只能放下其中的一部分
                         //自身的id != pickedItem.id       pickedItem跟当前物品交换
                   //2.IsPickedItem == false
                            //ctrl按下        取得当前物品槽中物品的一半
                            //ctrl没有按下    把当前物品里面的物品放到鼠标上


        if (transform.childCount > 0)
        {
            ItemUI currenItemui = transform.GetChild(0).GetComponent<ItemUI>();
          
            if (InventoryManager.Instance.IsPickedItem == false) // 当前鼠标上没有任何物品
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int pickedAmount = (currenItemui.Amount + 1) / 2;
                    InventoryManager.Instance.PickUpItem(currenItemui.Item, pickedAmount);
                    int pickedAmoumtSurplus = currenItemui.Amount - pickedAmount;
                    if(pickedAmoumtSurplus <= 0)
                    {
                        Destroy(currenItemui.gameObject); //销毁当前物品
                    }
                    else
                    {
                        currenItemui.SetItemAmount(pickedAmoumtSurplus);
                    }

                    

                }
                else
                {

                    InventoryManager.Instance.PickUpItem(currenItemui.Item,currenItemui.Amount);//把当前物品的信息，复制费pickediem（跟随鼠标)
                    Destroy(currenItemui.gameObject); // 销毁当前物品
                }
            }
            else
            {
                //TODO鼠标上存在物品
                if(currenItemui.Item.ID == InventoryManager.Instance.PickedItem.Item.ID)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currenItemui.Item.Capacity > currenItemui.Amount) //当前物品还有容量可以增加
                        {
                            currenItemui.AddAmount();
                            InventoryManager.Instance.RemoveItem();
                        }else
                        {
                            return;
                        }
                    }else
                    {
                        if (currenItemui.Item.Capacity > currenItemui.Amount)
                        {
                            int amountRemain = currenItemui.Item.Capacity - currenItemui.Amount; //当前物品剩余的空间
                            if (amountRemain >= InventoryManager.Instance.PickedItem.Amount)
                            {
                                currenItemui.SetItemAmount(currenItemui.Amount + InventoryManager.Instance.PickedItem.Amount);
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                            } else
                            {
                                currenItemui.SetItemAmount(currenItemui.Amount + amountRemain);
                                InventoryManager.Instance.RemoveItem(amountRemain);
                            }
                        }else
                        {
                            return;
                        }
                    }
                }else
                {
                    //鼠标上的物品和格子中的物品交换
                    Item item = currenItemui.Item;
                    int amount = currenItemui.Amount;

                    currenItemui.SetItem(InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(item, amount);
                }
            }
        }else
        {

            //自身是空  1. IsPickedItem = true       pickedItem放在这个位置
                                    // 按下ctrl      放置当前鼠标上的物品的一个
                                    // 没有按下ctrl  放置当前鼠标上的所有物品
                    // 2. IsPickedItem == false 不做任何处理

            if (InventoryManager.Instance.IsPickedItem == true)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem();
                }
                else
                {
                    for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }
            }else
            {
                return;
            }
        }

    }
}
