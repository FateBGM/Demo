using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgePanel : Inventory
{


    #region
    private static ForgePanel _instance;

    public static ForgePanel Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("ForgePanel").GetComponent<ForgePanel>();
            }
            return _instance;
        }
    }
    #endregion

    private List<Formula> formulaList;

    void Start()
    {
        base.Start();
        ParseFormulaJson();
    }

    void ParseFormulaJson()
    {
        formulaList = new List<Formula>();

        TextAsset forgelistText = Resources.Load<TextAsset>("ResJson/Formula");
        string formulaLison = forgelistText.text; //锻造配方的Json字符串
        JSONObject jb = new JSONObject(formulaLison);
        foreach (JSONObject temp in jb.list)
        {
            int item1Id = (int)temp["Item1Id"].n;
            int item1Amount = (int)temp["Item1Amount"].n;
            int item2Id = (int)temp["Item2Id"].n;
            int item2Amount = (int)temp["Item2Amount"].n;
            int resId = (int)temp["ResId"].n;

            Formula formula = null;
            formula = new Formula(item1Id, item1Amount, item2Id, item1Amount, resId);
            formulaList.Add(formula);
        }

        //Debug.Log(formulaList.ToString());

    }


    public void ForgeItem()
    {
        //得到当前有那些材料
        //判断端在满足哪个要求
        List<int> haveMaterialIdList = new List<int>(); //存储当前放入的有那些材料的ID
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI currentItemUi = slot.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < currentItemUi.Amount; i++)
                {
                    haveMaterialIdList.Add(currentItemUi.Item.ID); //这个格子里面有多少个物品就存储多少个id
                }
            }
        }

        Formula matchFormula = null;
        foreach (Formula formula in formulaList)
        {
            bool isMatch = formula.Match(haveMaterialIdList);
            if (isMatch)
            {
                matchFormula = formula; break;
            }
        }
        if (matchFormula != null)
        {
            Knapsack.Instance.StoreItem(matchFormula.ResId);
            //去掉消耗的材料
            foreach (int id in matchFormula.NeedIdList)
            {
                foreach (Slot slot in slotList)
                {
                    if (slot.transform.childCount > 0)
                    {
                        ItemUI itemui = slot.transform.GetChild(0).GetComponent<ItemUI>();
                        if (itemui.Item.ID == id && itemui.Amount >0)
                        {
                            itemui.ReducAmount(); 
                            if (itemui.Amount <= 0)
                            {
                                DestroyImmediate(itemui.gameObject);
                            }
                            break;
                        }
                    }
                }
            }
        }
   
    }

}
