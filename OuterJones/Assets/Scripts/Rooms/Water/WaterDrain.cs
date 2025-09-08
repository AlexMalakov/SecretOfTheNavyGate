using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrain : Floodable
{
    private bool reachedByFlood;
    private bool reachedByDrain;

    [SerializeField] Canal origin;
    [SerializeField] Canal drainTo;

    public void Awake() {
       FindObjectOfType<DrainManager>().addDrain(this);
    }

    public override void onFlood() {
        this.reachedByFlood = true;
    }

    public override void drainWater() {
        if(reachedByFlood || reachedByDrain) {
            return;
        }

        reachedByFlood = false;
        reachedByDrain = true;
        origin.drainWater(null);

        if(this.drainTo != null) {
            this.drainTo.onFlood(null);
        }
    }

    public void reset() {
        this.reachedByFlood = false;
        this.reachedByDrain = false;
    }
}
