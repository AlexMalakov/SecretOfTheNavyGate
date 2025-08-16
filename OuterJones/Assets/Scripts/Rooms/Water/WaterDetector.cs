using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour, Floodable
{
    private bool flooded;
    [SerializeField] private GameObject effectableObj;

    public override void onFlood() {
        flooded = true;
        this.effectableObj.GetComponent<Effectable>().onEffect();
    }

    public override void drainWater() {
        flooded = false;
    }

    public bool isFlooded() {
        return this.flooded;
    }
}
