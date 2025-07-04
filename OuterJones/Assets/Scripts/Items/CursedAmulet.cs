using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedAmulet : Item
{
    private Button[] allButtons;

    public void Start() {
        allButtons = FindObjectsOfType<Button>(true);
    }

    public override void equip() {
        foreach(Button b in this.allButtons) {
            b.setMummyButtonStatus(!b.getMummyStatus());
        }
    }

    public override void unequip() {
        foreach(Button b in this.allButtons) {
            b.setMummyButtonStatus(!b.getMummyStatus());
        }
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Amulet;
    }

}
