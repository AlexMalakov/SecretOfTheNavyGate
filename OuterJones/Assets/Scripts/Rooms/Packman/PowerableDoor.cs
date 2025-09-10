using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableDoor : GateDoor, Effectable
{


    [SerializeField] private bool initiallyOpen;
    [SerializeField] private bool dontToggle;
    [SerializeField] private UnityEngine.AI.NavMeshObstacle obstacle;


    protected override void Awake() {
        this.openState  = initiallyOpen;
        base.Awake();
        obstacle.enabled = true;
    }

    public void onEffect() {
        if(dontToggle) {
            this.opencloseDoor(true);
        } else {
            this.toggleOpen();
            obstacle.enabled = false;
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
        open.SetActive(openState);
        horizClosed.SetActive(!openState && !vert);
        vertClosed.SetActive(!openState && vert);
        obstacle.enabled = !openDesired;
    }

}
