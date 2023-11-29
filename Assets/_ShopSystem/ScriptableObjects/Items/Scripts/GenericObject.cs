using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Generic Object", menuName = "Inventory/Items/Generic")]
public class GenericObject : ItemObject
{
    private void Awake()
    {
        //make sure correct type is set
        _type = ItemType.Generic;
    }
}
