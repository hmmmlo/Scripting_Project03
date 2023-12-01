using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public int _currency = 500; //player currency
    [SerializeField] public List<ShopItem> _shopItems = new List<ShopItem>();
    private ShopItem _currentItem;

    //item references
    [Header("References")]
    public InventoryObject _playerInventory;
    public TextMeshProUGUI _currencyText;
    public GameObject shopUI;
    public GameObject _itemInfoPanel;
    public Transform shopContent;
    public GameObject _itemPrefab;

    [HideInInspector] public bool firstClick = false;

    private void Awake()
    {
        //add items from inspector to shop item cards
        foreach(ShopItem shopItem in _shopItems)
        {
            GameObject newItem = Instantiate(_itemPrefab, shopContent); //instantiate shop item on item card

            shopItem._itemRef = newItem; //set reference to item

            shopItem.SetValues(); //set values of item based on scriptable object information

            //update UI elements to correct variable from shop items
            foreach(Transform child in newItem.transform)
            {
                if(child.gameObject.name == "ItemName_txt") //item name
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._name.ToString();
                }
                else if (child.gameObject.name == "ItemSprite_img") //item sprite
                {
                    child.gameObject.GetComponent<Image>().sprite = shopItem._image;
                }
                else if (child.gameObject.name == "Cost_txt") //item price
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._cost.ToString();
                }
                else if (child.gameObject.name == "Quantity_txt") //item quantity
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._qty.ToString();
                }
            }

            newItem.GetComponent<Button>().onClick.AddListener(() => {
                UpdateItemInfo(shopItem); //show more item info in shop UI
            });
        }
    }

    public void BuyItem()
    {
        //checks if item in stock
        if(_currentItem._qty >= 1)
        {
            //checks if player has enough money
            if (_currency >= _currentItem._cost)
            {
                _currency -= _currentItem._cost;
                _playerInventory.AddItem(_currentItem._item, 1);
                _currentItem._qty -= 1;

                //update quantity text on shop item
                _currentItem._itemRef.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _currentItem._qty.ToString();
            }
        }
        else //prints console line if out of stock
        { 
            Debug.Log("out of stock"); 
        }
    }

    //function to toggle shop ui on or off, can call from other code
    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf); //changes active state to opposite of current for toggle
    }

    private void OnGUI()
    {
        _currencyText.text = _currency.ToString(); //set coin text

    }

    //update the item info panel to the right of the shop 
    public void UpdateItemInfo(ShopItem currentItem)
    {
        _currentItem = currentItem;

        if (firstClick)
        {
            _itemInfoPanel.SetActive(true);
        }
        foreach (Transform child in _itemInfoPanel.transform)
        {
            if (child.gameObject.name == "Info_ItemName_txt") //item name
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._name.ToString();
            }
            else if (child.gameObject.name == "Info_ItemImage_txt") //item sprite
            {
                child.gameObject.GetComponent<Image>().sprite = currentItem._image;
            }
            else if (child.gameObject.name == "Info_Qty_txt") //item quantity
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._qty.ToString();
            }
            else if (child.gameObject.name == "Info_ItemDesc_txt") //item price
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._description.ToString();
            }
            else if (child.gameObject.name == "Info_PlayerAmt_txt") //item price
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._playerAmt.ToString();
            }
        }
    }
}

//shop item information
[System.Serializable]
public class ShopItem
{
    public ItemObject _item;
    [HideInInspector] public string _name;
    [HideInInspector] public int _cost;
    [HideInInspector] public Sprite _image;
    [HideInInspector] public string _description;
    public int _qty;
    [HideInInspector] public GameObject _itemRef;
    [HideInInspector] public int _playerAmt;

    public void SetValues()
    {
        _name = _item._itemName;
        _cost = _item._itemPrice;
        _image = _item._itemSprite;
        _description = _item._description;
    }
}