using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSink : MonoBehaviour, Floodable
{
    private bool flooded = false;

    public void onFlood() {
        this.flooded = true;
    }

    public void drainWater() {
        this.flooded = false;
    }

    public bool isActive() {
        return this.flooded;
    }
}
