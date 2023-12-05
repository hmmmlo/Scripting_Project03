using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    public GameObject shopUI; //scrollview
    public Transform shopContent; //place where items spawn
    public GameObject _itemInfoPanel; //item info panel, right had side of shop
    public GameObject _shopCardPrefab; //item card to instantiate in the scrollview content area

    [HideInInspector] public bool firstClick = false; //used to know when to activate info panel

    private void Awake()
    {
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
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._qty.ToString();
                }
            }

            for (int i = 0; i < _playerInventory.Inventory.Count; i++)
            {
                if (_playerInventory.Inventory[i]._item == shopItem._item)
                {
                    shopItem._playerAmt = _playerInventory.Inventory[i]._amt;
                }
            }

            newItem.GetComponent<Button>().onClick.AddListener(() => {
                if(firstClick == false) //if item never been clicked before
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

        for (int i = 0; i < _playerInventory.Inventory.Count; i++)
        {
            if (_playerInventory.Inventory[i]._item == _currentItem._item)
            {
                _currentItem._playerAmt = _playerInventory.Inventory[i]._amt;
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
        _currencyText.text = _currency.ToString(); //set coin text & update
        if (_currentItem != null)
        {
            _ownedText.text = _currentItem._playerAmt.ToString(); //set currently owned text & update 
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
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._qty.ToString();
            }
            else if (child.gameObject.name == "Info_ItemDesc_txt") //item price
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._description.ToString();
            }
           /*else if (child.gameObject.name == "Info_PlayerAmt_txt") //player amount
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = currentItem._playerAmt.ToString();
            }*/
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