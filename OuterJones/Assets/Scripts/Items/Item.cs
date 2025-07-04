using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private Sprite iconSprite;

    public virtual void equip() {

    }

    public virtual void unequip() {

    }

    public virtual void onGain() {

    }
    
    public abstract PossibleItems getItemType();

    public Sprite getItemIcon() {
        return this.iconSprite;
    }
}
