using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Basic Attribute
    private int basicStrenth = 10;
    private int basicIntellect = 10;
    private int basicAgility = 10;
    private int basicStamina = 10;
    private int basicDamage = 10;

    public int BasicStrenth { get { return basicStrenth; } }
    public int BasicIntellect { get { return basicIntellect; } }
    public int BasicAgility { get { return basicAgility; } }
    public int BasicStamina { get { return basicStamina; } }
    public int BasicDamage { get { return basicDamage; } }
    #endregion

    private int coinAmount = 1000;

    private Text coinText;

    public int CoinAmount
    {
        get
        {
            return coinAmount;
        }
        set
        {
            coinAmount = value;
            coinText.text = coinAmount.ToString();
        }
    }

    void Start()
    {
        coinText = GameObject.Find("Coin").GetComponentInChildren<Text>();
        coinText.text = coinAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //G 随机得到一个物品放到背包
        if (Input.GetKeyDown(KeyCode.G))
        {
            int id = Random.Range(1, 18);
            //Debug.Log(id);
            Knapsack.Instance.StoreItem(id);
        }

        //T 控制背包的显示和隐藏
        if (Input.GetKeyDown(KeyCode.T))
        {
            Knapsack.Instance.DisplaySwitch();
        }

        //Y 控制箱子的显示和隐藏
        if (Input.GetKeyDown(KeyCode.Y))
        {

            Chest.Instance.DisplaySwitch();
        }

        //B 控制装备面板的显示和隐藏
        if (Input.GetKeyDown(KeyCode.E))
        {
            CharacterPanel.Instance.DisplaySwitch();
        }

        //I 控制商店显示和隐藏
        if (Input.GetKeyDown(KeyCode.I))
        {
            VendorPanel.Instance.DisplaySwitch();
        }

        //F 控制锻造面板的显示和隐藏
        if (Input.GetKeyDown(KeyCode.F))
        {
            ForgePanel.Instance.DisplaySwitch();
        }

    }

    /// <summary>
    /// 消费
    /// </summary>
    /// <param name="amount"></param>
    public bool ConsumeCoin(int amount)
    {
        if (coinAmount >= amount)
        {
            coinAmount -= amount;
            coinText.text = coinAmount.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 赚取金币
    /// </summary>
    /// <param name="amount"></param>
    public void EarnCoin(int amount)
    {
        this.coinAmount += amount;
        coinText.text = coinAmount.ToString();
    }
}
