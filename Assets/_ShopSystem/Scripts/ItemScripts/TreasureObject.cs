using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Treasure Object", menuName = "Inventory/Items/Treasure")]
public class TreasureObject : ItemObject
{
    private void Awake()
    {
        //make sure correct type is set
        _type = ItemType.Treasure;
    }
}
