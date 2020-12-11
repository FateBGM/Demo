using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    protected Slot[] slotList;

    private float targetAlpha = 1;

    private float smoothing = 5;

    private CanvasGroup canvasGroup;

    public virtual void Start()
    {
        slotList = GetComponentsInChildren<Slot>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(targetAlpha - canvasGroup.alpha) < 0.01f)
                {  
                canvasGroup.alpha = targetAlpha;

            }
        }
    }

    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);
        //Debug.Log("StoreItem111=" + item);
        return StoreItem(item);
    }

    /// <summary>
    /// 这个方法用来哦找到一个空的物品槽
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StoreItem(Item item)
    {
        //Debug.Log("StoreItem222=" + item);
        if (item == null)
        {
            //Debug.LogWarning("要储存的物品ID不存在");
            return false;
        }
        if (item.Capacity == 1)
        {
            //TODO
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                //Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                slot.StoreItem(item);//把物品储存到这个空的物品槽里面
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(item);
            if (slot != null)
            {
                slot.StoreItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    //Debug.Log("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 这个方法用来找到一个空的物品槽
    /// </summary>
    private Slot FindEmptySlot()
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;

    }

    private Slot FindSameIdSlot(Item item)
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ID && slot.IsFilled() == false)
            {
                return slot;
            }

        }
        return null;
    }


    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        targetAlpha = 1;
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        targetAlpha = 0;
    }

    public void DisplaySwitch()
    {
        if(targetAlpha == 0)
        {
            Show();
        }else
        {
            Hide();
        }
    }

    #region save and load
    public void SaveInventory()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Slot slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                sb.Append(itemUI.Item.ID + "," + itemUI.Amount + "-");
            }else
            {
                sb.Append("0-");
            }
            
        }
        PlayerPrefs.SetString(this.gameObject.name, sb.ToString());
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(this.gameObject.name) == false) return;

        string str = PlayerPrefs.GetString(this.gameObject.name);
        string[] itemArray = str.Split('-');
        for (int i = 0; i <itemArray.Length-1; i++)
        {
            string itemstr = itemArray[i];
            if(itemstr != "0")
            {
                string[] temp = itemstr.Split(',');
                int id = int.Parse(temp[0]);
                Item item = InventoryManager.Instance.GetItemById(id);
                int amount = int.Parse(temp[1]);
                for (int j = 0; j < amount; j++)
                {
                    slotList[i].StoreItem(item);
                }
            }
        }

    }


    #endregion
}
