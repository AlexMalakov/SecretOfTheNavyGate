using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : Floodable
{
    private bool flooded;
    [SerializeField] private GameObject effectableObj;

    public override void onFlood(bool fromSource) {
        if(!this.flooded) {
            flooded = true;
            this.effectableObj.GetComponent<Effectable>().onEffect();
        }
    }

    public override void drainWater() {
        if(this.flooded) {
            flooded = false;
            this.effectableObj.GetComponent<Effectable>().onEffectOver();
        }
    }

    public bool isFlooded() {
        return this.flooded;
    }
}
