using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public int _currency = 500; //player currency
    [SerializeField] private List<ShopItem> _shopItems = new List<ShopItem>();

    //item references
    [Header("References")]
    public TextMeshProUGUI _currencyText;
    public GameObject shopUI;
    public Transform shopContent;
    public GameObject _itemPrefab;

    private void Awake()
    {
        //make sure this is the only shop manager in the scene
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(ShopItem shopItem in _shopItems)
        {
            GameObject newItem = Instantiate(_itemPrefab, shopContent);

            shopItem._itemRef = newItem;

            shopItem.SetValues();

            foreach(Transform child in newItem.transform)
            {
                if(child.gameObject.name == "ItemName_txt")
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._name.ToString();
                }
                else if (child.gameObject.name == "ItemSprite_img")
                {
                    child.gameObject.GetComponent<Image>().sprite = shopItem._image;
                }
                else if (child.gameObject.name == "Cost_txt")
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._cost.ToString();
                }
                else if (child.gameObject.name == "Quantity_txt")
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = shopItem._qty.ToString();
                }
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
        _currencyText.text = _currency.ToString(); //set coin text
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

    public void SetValues()
    {
        _name = _item._itemName;
        _cost = _item._itemPrice;
        _image = _item._itemSprite;
        _description = _item._description;
    }
}