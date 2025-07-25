using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Item
{
    private GraplePoint[] allPoints;

    public void Awake() {
        allPoints = FindObjectsOfType<GraplePoint>(true);
    }


    public override void equip() {
        foreach(GraplePoint p in this.allPoints) {
            p.setWhipStatus(true);
        }
    }

    public override void unequip() {
        foreach(GraplePoint p in this.allPoints) {
            p.setWhipStatus(false);
        }
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Whip;
    }

}
