using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public virtual void equip() {

    }

    public virtual void unequip() {

    }

    public virtual void onGain() {

    }
    
    public abstract PossibleItems getItemType();
}
