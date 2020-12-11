using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{

    #region Data
    public Item Item { get; private set; }
    public int Amount { get; private set; }
    #endregion

    #region UI Component
    private Image itemImage;
    private Text amountImageText;


    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                itemImage = GetComponent<Image>();

            }
            return itemImage;
        }
    }

    private Text AmountImageText
    {
        get
        {
            if (amountImageText == null)
            {
                amountImageText = GetComponentInChildren<Text>();
            }
            return amountImageText;
        }
    }
    #endregion


    private float targetScale = 1f;

    private Vector3 animation = new Vector3(1.5f, 1.5f, 1.5f);

    private float smoothing = 3;

    void Update()
    {
        if (transform.localScale.x != targetScale)
        {
            //动画
            float scale = Mathf.Lerp(transform.localScale.x, targetScale, smoothing * Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            if(Mathf.Abs(transform.localScale.x - targetScale) < 0.2f){
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            }
       
        }
    }

    public void SetItem(Item item, int amount = 1)
    {
        transform.localScale = animation;
        this.Item = item;

        this.Amount = amount;

        //updata ui
        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite);

        if (Item.Capacity > 1)
        {
            AmountImageText.text = Amount.ToString();

        }
        else
        {
            AmountImageText.text = "";

        }

    }

    public void SetItemAmount(int amount)
    {
        transform.localScale = animation;
        this.Amount = amount;

        if (Item.Capacity > 1)
        {
            AmountImageText.text = Amount.ToString();
        }
        else
        {
            AmountImageText.text = "";
        }

    }

    public void AddAmount(int amount = 1)
    {
        transform.localScale = animation;
        this.Amount += amount;
        //updata ui TODO
        if (Item.Capacity > 1)
        {
            AmountImageText.text = Amount.ToString();
        }
        else
        {
            AmountImageText.text = "";
        }


    }

    public void ReducAmount(int amount =1)
    {
        transform.localScale = animation;
        this.Amount -= amount;
        //updata ui TODO
        if (Item.Capacity > 1)
        {
            AmountImageText.text = Amount.ToString();
        }
        else
        {
            AmountImageText.text = "";
        }

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void SetLocalPostion(Vector3 postion)
    {
        transform.localPosition = postion;
    }

    //当前物品 和另外一个物品交换显示
    public void ExChange(ItemUI itemUI)
    {
        Item itemTemp = itemUI.Item;
        int amountTemp = itemUI.Amount;

        itemUI.SetItem(this.Item, this.Amount);
        this.SetItem(itemTemp,amountTemp);
    }


}
