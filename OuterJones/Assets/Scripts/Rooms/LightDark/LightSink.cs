using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSink : MonoBehaviour
{
    private bool beamed = false;
    [SerializeField] private DoorDirection receiveBeamFrom;
    [SerializeField] private Effectable effectable;
    [SerializeField] private GameObject beamedSprite;
    [SerializeField] private GameObject notBeamedSprite;

    public void activate(DoorDirection beamFrom) {
        if(Door.rotateDoorDirection(Door.rotateDoorDirection(beamFrom, true), true) == this.receiveBeamFrom) {
            this.beamed = true;
            this.effectable.onEffect();
            this.beamedSprite.SetActive(true);
            this.beamedSprite.SetActive(false);
        }
    }

    public void deactivate() {
        this.beamed = false;
        this.beamedSprite.SetActive(false);
        this.beamedSprite.SetActive(true);
    }

    public bool getActive() {
        return this.beamed;
    }

    public void rotate90(bool clockwise) {
        this.receiveBeamFrom = Door.rotateDoorDirection(this.receiveBeamFrom, clockwise);
    }

}
