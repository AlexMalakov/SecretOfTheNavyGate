using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrain : Floodable
{
    private bool reachedByFlood;
    private bool reachedByDrain;
    private bool flooded;

    [SerializeField] Canal origin;
    [SerializeField] Canal drainTo;

    public void Awake() {
       FindObjectOfType<DrainManager>().addDrain(this);
    }

    public override void onFlood(bool fromSource) {
        // Debug.Log("DRAINED REACHED BY FLOOD:" + Time.time + "I AM " + gameObject.name);
        this.reachedByFlood = fromSource;
        this.flooded = true;
    }

    public override void drainWater() {
        // Debug.Log("DRAINING DRAIN:" + Time.time + "I AM " + gameObject.name);
        if(reachedByFlood || reachedByDrain) {
            return;
        }
        
        reachedByFlood = false;
        reachedByDrain = true;
        origin.drainWater(null);

        if(this.flooded && this.drainTo != null) {
            this.drainTo.onFlood(null, false);
        }

        this.flooded = false;
    }

    public void reset() {
        // Debug.Log("RESSETTING DRAIN:" + Time.time + "I AM " + gameObject.name);
        this.reachedByFlood = false;
        this.reachedByDrain = false;
    }
}
