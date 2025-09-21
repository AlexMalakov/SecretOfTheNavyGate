using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableDoor : GateDoor, Effectable
{


    [SerializeField] private bool initiallyOpen;
    [SerializeField] private bool dontToggle;

    protected override void Awake() {
        this.openState = initiallyOpen;
        base.Awake();
    }

    public void onEffect() {
        if(dontToggle) {
            this.opencloseDoor(true);
        } else {
            this.toggleOpen();
        }
    }

    public void onEffectOver() {
        if(dontToggle) {
            this.opencloseDoor(false);
        }
    }

    //if specifically open/close is wanted
    public void opencloseDoor(bool openDesired) {
        openState = openDesired;
        this.flipSprites();
    }

}
