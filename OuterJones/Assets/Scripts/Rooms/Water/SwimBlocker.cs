using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimBlocker : Floodable
{
    [SerializeField] private GameObject floodCollider;


    public void Awake() {
        this.floodCollider.SetActive(false);
    }

    public override void onFlood() {
        this.floodCollider.SetActive(true);
    }

    public override void drainWater() {
        this.floodCollider.SetActive(false);
    }


}
