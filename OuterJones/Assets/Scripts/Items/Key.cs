using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField] private int keyCount;

    public bool hasKey() {
        return this.keyCount > 0;
    }

    public void useKey() {
        this.keyCount--;
        
        //update hotbar to show one less key
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Key;
    }

    public void gainKey() {
        this.keyCount++;
    }
}
