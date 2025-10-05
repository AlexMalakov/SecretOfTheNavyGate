using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSink : Floodable
{
    private bool flooded = false;

    public override void onFlood(bool fromSource) {
        this.flooded = true;
    }

    public override void drainWater() {
        this.flooded = false;
    }

    public bool isActive() {
        return this.flooded;
    }
}
