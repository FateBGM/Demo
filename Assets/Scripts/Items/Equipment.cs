using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 装备
/// </summary>
public class Equipment : Item
{
    /// <summary>
    /// 力量
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellet { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get; set; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get; set; }
    /// <summary>
    /// 装备类型
    /// </summary>
    public EquipmentType EquipmentTypes { get; set; }


    public Equipment(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite, int strength, int intellet, int agility, int stamina, EquipmentType equipmentTypes) : 
        base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Strength = strength;
        this.Intellet = intellet;
        this.Agility = agility;
        this.Stamina = stamina;
        this.EquipmentTypes = equipmentTypes;
    }

    public enum EquipmentType
    {
        //无
        None,
        //头
        Head,
        //脖子
        Neck,
        //胸部
        Chest,
        //戒子
        Ring,
        //腿
        Leg,
        //护腕
        Bracer,
        //鞋子
        Boots,
        //饰品
        //Trinket,
        //肩膀
        Shoulder,
        //腰带
        Belt,
        //副手
        OffHand

    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();

        string equipTypeText = "";
        switch (EquipmentTypes)
        {
            case EquipmentType.Head:
                equipTypeText = "头部";
                break;
            case EquipmentType.Neck:
                equipTypeText = "脖子";
                break;
            case EquipmentType.Chest:
                equipTypeText = "胸部";
                break;
            case EquipmentType.Ring:
                equipTypeText = "戒指";
                break;
            case EquipmentType.Leg:
                equipTypeText = "腿部";
                break;
            case EquipmentType.Bracer:
                equipTypeText = "护腕";
                break;
            case EquipmentType.Boots:
                equipTypeText = "靴子";
                break;
            case EquipmentType.Shoulder:
                equipTypeText = "护肩";
                break;
            case EquipmentType.Belt:
                equipTypeText = "腰带";
                break;
            case EquipmentType.OffHand:
                equipTypeText = "副手";
                break;
            default:
                break;
        }

        string newText = string.Format("{0}\n\n<color=blue>装备类型:{1}\n力量：{2}\n智力：{3}\n敏捷：{4}\n体力：{5}</color>", text, equipTypeText, Strength, Intellet, Agility,Stamina);
        return newText;
    }
}
