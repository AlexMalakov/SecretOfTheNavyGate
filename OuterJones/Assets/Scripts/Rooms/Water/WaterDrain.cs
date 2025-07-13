using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrain : Floodable
{
    private bool reachedByFlood;

    [SerializeField] Canal origin;

    public void Awake() {
       FindObjectOfType<DrainManager>().addDrain(this);
    }

    public override void onFlood() {
        this.reachedByFlood = true;
    }

    public override void drainWater() {
        if(reachedByFlood) {
            return;
        }

        reachedByFlood = false;
        origin.drainWater(null);
    }

    public void reset() {
        this.reachedByFlood = false;
    }

}
