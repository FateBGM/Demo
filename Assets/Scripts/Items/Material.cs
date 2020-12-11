using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 材料类
/// </summary>
public class Material : Item
{
    public Material(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite) 
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
    }
}
