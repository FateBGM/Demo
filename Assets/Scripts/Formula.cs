using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{
   
    public int Item1Id { get;private set; }
    public int Item1Amount { get;private set; }
    public int Item2Id { get;private set; }
    public int Item2Amount { get;private set; }

    public int ResId { get; set; } //锻造结果物品

    private List<int> needIdList = new List<int>(); //所需要物品ID的集合

    public List<int> NeedIdList
    {
        get
        {
            return needIdList;
        }
    }

    public Formula(int item1Id,int item1Amount,int item2Id ,int item2Amount,int resId)
    {
        this.Item1Id = item1Id;
        this.Item1Amount = item1Amount;
        this.Item2Id = item2Id;
        this.Item2Amount = item2Amount;
        this.ResId = resId;

        for (int i = 0; i < Item1Amount; i++)
        {
            needIdList.Add(Item1Id);
        }
        for (int i = 0; i < Item2Amount; i++)
        {
            needIdList.Add(Item2Id);
        }
    }

    public bool Match(List<int> idList) 
    {
      
        List<int> tempIdList = new List<int>(idList);
        foreach (int id in needIdList)
        {
            bool isSuccess = tempIdList.Remove(id);
            if(isSuccess == false)
            {
                return false;
            }
        }
        return true;

    }
}
