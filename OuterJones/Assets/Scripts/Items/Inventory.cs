using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PossibleItems {
    Amulet, Floaties, GearItem, Torch, Whip, Key
}
public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    [SerializeField] private int equipedItem = -1;

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1) && items.Count > 0) {
            equipItemN(0);
        } else if(Input.GetKeyDown(KeyCode.Alpha2) && items.Count > 0) {
            equipItemN(1);
        } else if(Input.GetKeyDown(KeyCode.Alpha3) && items.Count > 0) {
            equipItemN(2);
        } else if(Input.GetKeyDown(KeyCode.Alpha4) && items.Count > 0) {
            equipItemN(3);
        } else if(Input.GetKeyDown(KeyCode.Alpha5) && items.Count > 0) {
            equipItemN(4);
        } else if(Input.GetKeyDown(KeyCode.Alpha6) && items.Count > 0) {
            equipItemN(5);
        }
    }

    private void equipItemN(int n) {
        if(this.equipedItem == n) {
            this.items[n].unequip();
            this.equipedItem = -1;
        } else {
            if(this.equipedItem >= 0) {
                this.items[n].unequip();
            }

            this.equipedItem = n;
            this.items[n].equip();
        }
    }

    public bool hasItem(PossibleItems t) {
        foreach(Item i in this.items) {
            if(i.getItemType() == t) {
                return true;
            }
        }
        return false;
    }
}
