using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour
{

    #region 单例模式
    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //代码执行一次
                _instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            }
            return _instance;
        }

    }
    #endregion

    /// <summary>
    /// 物品信息列表（集合）
    /// </summary>
    private List<Item> itemList;

    #region ToolTip
    private ToolTip toolTip;

    public bool isToolTipShow = false;

    private Vector2 toolTipPositionOffest = new Vector2(10, -10);
    #endregion 

    private Canvas canvas;

    #region PidkedItem
    private bool ispickedItem = false;

    public bool IsPickedItem
    {
        get
        {
         
            return ispickedItem;
        }
        //set
        //{
          
        //    ispickedItem = value;
        //}
    }

    private ItemUI pidkedItem; //鼠标选中的物体

    public ItemUI PickedItem
    {
        get
        {
            return pidkedItem;
        }
    
    }
    #endregion

    void Awake()
    {
        ParseItemsJson();
    }

    void Start()
    {
 
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pidkedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        pidkedItem.Hide();
    }

    void Update()
    {
        if (ispickedItem)
        {
            //捡起物品，让物品跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pidkedItem.SetLocalPostion(position);
        }
        else
        if (isToolTipShow)
        {
            //控制提示面板中提示跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetLocalPosition(position + toolTipPositionOffest);
        }

        //物品丢弃的处理
        if(ispickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) ==false)
        {
            ispickedItem = false;
            PickedItem.Hide();
        }
    }

    /// <summary>
    /// 解析物品信息
    /// </summary>
    void ParseItemsJson()
    {
        itemList = new List<Item>();

        TextAsset itemText = Resources.Load<TextAsset>("ResJson/Items");
        string itemJson = itemText.text;

        JSONObject js = new JSONObject(itemJson);

        foreach (JSONObject temp in js.list)
        {
            //Debug.Log(temp["id"].ToString()+ temp["name"].ToString());
            string typeStr = temp["type"].str;
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), typeStr);

            //下面的是解析这个独享的共有属性
            int id = (int)(temp["id"].n);
            //Debug.Log("ParseItemsJson id=" + id);
            string name = temp["name"].str;
            Item.ItemQuality quality = (Item.ItemQuality)System.Enum.Parse(typeof(Item.ItemQuality), temp["quality"].str);
            string description = temp["description"].str;
            int capacity = (int)(temp["capacity"].n);
            int buyPrice = (int)(temp["buyPrice"].n);
            int sellPrice = (int)(temp["sellPrice"].n);
            string sprite = temp["sprite"].str;

            Item item = null;
            switch (type)
            {
                case Item.ItemType.Consumable:
                    int hp = (int)(temp["hp"].n);
                    int mp = (int)(temp["mp"].n);
                    item = new Consumable(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, hp, mp);
                    break;
                case Item.ItemType.Equipment:
                    //TODO
                    int strength = (int)temp["strength"].n;
                    //Debug.Log("strength="+strength);
                    int intellect = (int)temp["intellect"].n;
                    //Debug.Log(intellect);
                    int agility = (int)temp["agility"].n;
                    //Debug.Log(agility);
                    int stamina = (int)temp["stamina"].n;
                    //Debug.Log(stamina);
                    Equipment.EquipmentType equipmentType = (Equipment.EquipmentType)System.Enum.Parse(typeof(Equipment.EquipmentType), temp["equipmentTypes"].str);
                    //Debug.Log(equipmentType);
                    item = new Equipment(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, strength, intellect, agility, stamina, equipmentType);
                    break;
                case Item.ItemType.Weapon:
                    //TODO
                    int damage = (int)temp["damage"].n;
                    Weapon.WeaponType wpType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), temp["WeaponType"].str);
                    item = new Weapon(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, damage, wpType);
                    break;
                case Item.ItemType.Material:
                    //TODO
                    item = new Material(id, name,type, quality, description, capacity, buyPrice, sellPrice, sprite);
                    break;
            }
            itemList.Add(item);
            //Debug.Log(item);

        }
    }


    public Item GetItemById(int id)
    {
        //Debug.Log("GetItemById =" + id);
        foreach (Item item in itemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTip(string content)
    {
        if (ispickedItem) return;
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }


    public void PickUpItem(Item item,int amount)
    {
        PickedItem.SetItem(item, amount);
        ispickedItem = true;

        PickedItem.Show();
        this.toolTip.Hide();
        //捡起物品，让物品跟随鼠标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        pidkedItem.SetLocalPostion(position);
    }

    /// <summary>
    /// 从Pickeditem拿掉一个物品放在物品槽里面
    /// </summary>
    public void RemoveItem(int amount=1)
    {
        PickedItem.ReducAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            ispickedItem = false;
            PickedItem.Hide();
        }
    }

    public void SaveInventory()
    {
        Knapsack.Instance.SaveInventory();
        Chest.Instance.SaveInventory();
        CharacterPanel.Instance.SaveInventory();
        ForgePanel.Instance.SaveInventory();
        PlayerPrefs.SetInt("CoinAmount", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount);
        
    }

    public void LoadInventory()
    {
        Knapsack.Instance.LoadInventory();
        Chest.Instance.LoadInventory();
        CharacterPanel.Instance.LoadInventory();
        ForgePanel.Instance.LoadInventory();
        if (PlayerPrefs.HasKey("CoinAmount"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount = PlayerPrefs.GetInt("CoinAmount");
        }
    }
}