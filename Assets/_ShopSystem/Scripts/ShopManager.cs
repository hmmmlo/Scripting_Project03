using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Audio;

public class ShopManager : MonoBehaviour
{
    public int _currency = 500; //player currency
    [SerializeField] public List<ShopItem> _shopItems = new List<ShopItem>(); //list to hold items in the shop
    private ShopItem _currentItem; //currently selected item

    //item references
    [Header("References")]
    public InventoryObject _playerInventory; //inventory scriptable object
    public TextMeshProUGUI _currencyText; //amount of money player has display
    public TextMeshProUGUI _ownedText; //player's owned item count display

    [Header("Item Display Area")]
    //item display area
    public GameObject shopUI; //scrollview
    public Transform shopContent; //place where items spawn
    public GameObject _shopCardPrefab; //item card to instantiate in the scrollview content area

    [Header("Shop Item Info")]
    //shop items
    public GameObject _itemInfoPanel; //item info panel, right had side of shop

    //extra info
    public GameObject _soldOutPanel; //if player tries to buy and theres no stock
    public GameObject _noMoneyPanel; //if player doesn't have enough money

    [HideInInspector] public bool firstClick = false; //used to know when to activate info panel

    private Color _textColor;

    private void Awake()
    {
        _textColor = _currencyText.color; //take in current text color
        _itemInfoPanel.SetActive(false); //panel turned off bc no item selected

        //add items from inspector to shop item cards
        foreach(ShopItem shopItem in _shopItems)
        {
            GameObject newItem = Instantiate(_shopCardPrefab, shopContent); //instantiate shop item on item card

            shopItem._itemRef = newItem; //set itemCard reference to item
            shopItem.SetValues(); //set values of item based on scriptable object information

            //update UI elements of shop itemCard to correct variable from shop items
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
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._quantity.ToString();
                }
            }

            for (int i = 0; i < _playerInventory.Inventory.Count; i++)
            {
                if (_playerInventory.Inventory[i]._item == shopItem._item)
                {
                    shopItem._playerAmt = _playerInventory.Inventory[i]._amount;
                }
            }

            newItem.GetComponent<Button>().onClick.AddListener(() => {
                if (firstClick == false) //if item never been clicked before
                {
                    firstClick = true; //first click has happened
                }

                UpdateItemInfo(shopItem); //show more item info in shop UI
            });
        }
    }

    public void BuyItem()
    {
        //checks if item in stock
        if(_currentItem._quantity >= 1)
        {
            //checks if player has enough money
            if (_currency >= _currentItem._cost)
            {
                _currency -= _currentItem._cost;
                _playerInventory.AddItem(_currentItem._item, 1);
                _currentItem._quantity -= 1;

                //update quantity text on shop item
                _currentItem._itemRef.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _currentItem._quantity.ToString();
            }
            else
            {
                _noMoneyPanel.SetActive(true); //tell player they don't have enough money
            }
        }
        else //prints console line if out of stock
        {
            _soldOutPanel.SetActive(true); //tell player the shop is sold out of item
            Debug.Log("out of stock");
        }

        //set current item's amount to player's inventory amount
        for (int i = 0; i < _playerInventory.Inventory.Count; i++)
        {
            if (_playerInventory.Inventory[i]._item == _currentItem._item)
            {
                _currentItem._playerAmt = _playerInventory.Inventory[i]._amount;
            }
        }
    }

    //function to toggle shop ui on or off, can call from other code
    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf); //changes active state to opposite of current for toggle
    }

    private void OnGUI()
    {
        // player currency display text

        if(_currency <= 0)
        {
            _currencyText.color = Color.red; //if 0 money display money in red color
        }
        else
        {
            _currencyText.color = _textColor; //else money is default text color
        }

        if (_currency >= 99999)
        {
            _currencyText.text = "99999"; //if money is over 99999 then cap the display text to that number to avoid wrapping issues
        }
        else //else set the currency text to whatever player has
        {
            _currencyText.text = _currency.ToString(); //set coin text & update
        }

        // player owned item count display text

        if (_currentItem != null)
        {
            if (_currentItem._playerAmt >= 99999)
            {
                _ownedText.text = "99999+"; //if item count is over 999999 then cap the display text to that number to avoid wrapping issues
            }
            else //else set the owned text to whatever player has
            {
                _ownedText.text = _currentItem._playerAmt.ToString(); //set currently owned text & update 
            }
        }
    }

    //update the item info panel to the right of the shop 
    public void UpdateItemInfo(ShopItem currentItem)
    {
        _currentItem = currentItem; //set current item for BuyItem function to access

        if (firstClick) //if item has been clicked turn on info panel
        {
            _itemInfoPanel.SetActive(true);
        }

        //update info panel with values
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
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._quantity.ToString();
            }
            else if (child.gameObject.name == "Info_ItemDesc_txt") //item price
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._description.ToString();
            }
        }
    }
}

//shop item information
[System.Serializable]
public class ShopItem
{
    //seen in inspector, user can set shop item and quantity
    public ItemObject _item;
    public int _quantity;

    //hide these from inspector, these get filled by item object / inventory object values
    [HideInInspector] public string _name;
    [HideInInspector] public int _cost;
    [HideInInspector] public Sprite _image;
    [HideInInspector] public string _description;
    [HideInInspector] public GameObject _itemRef;
    [HideInInspector] public int _playerAmt;

    //function to set values to shop item variables
    public void SetValues()
    {
        _name = _item._itemName;
        _cost = _item._itemPrice;
        _image = _item._itemSprite;
        _description = _item._description;
    }
}