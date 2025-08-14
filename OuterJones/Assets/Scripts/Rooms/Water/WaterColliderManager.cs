using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColliderManager : MonoBehaviour, ItemListener
{

    private bool floatiesAquired;

    private bool onGrate;

    private bool onBridge;

    public void Awake() {
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Floaties, this);
    }

    public void onItemEvent(bool itemStatus) {
        this.floatiesAquired = true;

        this.setColliderTriggerStatus();
    }

    public void setBridgeStatus(bool status) {
        this.onBridge = status;

        this.setColliderTriggerStatus();
    }

    public void setGrateStatus(bool status) {
        this.onGrate = status;

        this.setColliderTriggerStatus();
    }

    private void setColliderTriggerStatus() {
        this.GetComponent<CompositeCollider2D>().isTrigger = floatiesAquired || onGrate || onBridge;
    }
}