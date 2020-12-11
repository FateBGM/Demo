using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器
/// </summary>
public class Weapon : Item
{
    /// <summary>
    ///   伤害    
    /// </summary>
    public int Damage { get; set; }
    /// <summary>
    /// 武器类型
    /// </summary>
    public WeaponType WeaponTypes { get; set; }


    public Weapon(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite, int damage, WeaponType weaponTypes) : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Damage = damage;
        this.WeaponTypes = weaponTypes;
    }

    public enum WeaponType
    {
        None,
        MainHand,
        OffHand

    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();

        string weaponTypeText = "";
        switch (WeaponTypes)
        {
            case WeaponType.MainHand:
                weaponTypeText = "主手";
                break;
            case WeaponType.OffHand:
                weaponTypeText = "副手";
                break;
            default:
                break;
        }

        string newText = string.Format("{0}\n\n<color=blue>装备类型:{1}\n伤害：{2}</color>", text, weaponTypeText,Damage);
        return newText;
    }
}
