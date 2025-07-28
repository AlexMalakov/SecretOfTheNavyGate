using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableDoor : GateDoor, Effectable
{


    [SerializeField] private bool initiallyOpen;
    [SerializeField] private bool dontToggle;


    protected override void Awake() {
        this.openState  = initiallyOpen;
        base.Awake();
    }

    public void onEffect() {
        if(dontToggle) {
            this.opencloseDoor(true);
        } else {
            this.toggleOpen();
        }
    }

    //if specifically open/close is wanted
    public void opencloseDoor(bool openDesired) {
        openState = openDesired;
        open.SetActive(openState);
        horizClosed.SetActive(!openState && !vert);
        vertClosed.SetActive(!openState && vert);
    }

}
