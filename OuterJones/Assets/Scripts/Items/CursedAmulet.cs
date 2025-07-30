using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedAmulet : Item
{
    private PowerableButton[] allButtons;

    public void Awake() {
        allButtons = FindObjectsOfType<PowerableButton>(true);
    }

    public override void equip() {
        foreach(PowerableButton b in this.allButtons) {
            b.setMummyButtonStatus(!b.getMummyStatus());
        }
    }

    public override void unequip() {
        foreach(PowerableButton b in this.allButtons) {
            b.setMummyButtonStatus(!b.getMummyStatus());
        }
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Amulet;
    }

}
