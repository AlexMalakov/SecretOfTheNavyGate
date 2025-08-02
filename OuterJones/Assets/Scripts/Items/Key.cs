using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField] private int keyCount;
    [SerializeField] private Inventory inventory;

    public bool hasKey() {
        return this.keyCount > 0;
    }

    public void useKey() {
        this.keyCount--;

        this.inventory.onKeyUpdate();
        //update hotbar to show one less key
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Key;
    }

    public int getKeyCount() {
        return this.keyCount;
    }

    public void gainKey() {
        this.keyCount++;
        this.inventory.onKeyUpdate();
    }
}
