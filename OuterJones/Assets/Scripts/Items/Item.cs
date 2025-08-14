using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemListener {
    void onItemEvent(bool itemStatus);
}

public abstract class Item : MonoBehaviour
{
    [SerializeField] private Sprite iconSprite;
    private bool equiped;

    public virtual void equip() {
        this.equiped = true;
    }

    public virtual void unequip() {
        this.equiped = false;
    }

    public virtual bool isEquiped() {
        return this.equiped;
    }
    
    public abstract PossibleItems getItemType();

    public Sprite getItemIcon() {
        return this.iconSprite;
    }

    public abstract bool startsEquiped();
    public abstract bool canBeToggled();
}
