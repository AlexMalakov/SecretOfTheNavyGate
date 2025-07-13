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
            Debug.Log("REACHED AGAIN?");
            return;
        }

        Debug.Log("ATTEMPTING DRAIN!");
        reachedByFlood = false;
        origin.drainWater(null);
    }

    public void reset() {
        this.reachedByFlood = false;
    }

}
