using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSink : MonoBehaviour
{
    private bool beamed = false;
    [SerializeField] private DoorDirection receiveBeamFrom;
    [SerializeField] private GameObject effectableObj;
    [SerializeField] private GameObject beamedSprite;
    [SerializeField] private GameObject notBeamedSprite;


    private LDPuzzle puzzle;
    private string id;

    public void init(LDPuzzle puzzle, string id) {
        this.puzzle = puzzle;
        this.id = id;
    }

    public void activate(DoorDirection beamFrom) {
        if(/*Door.rotateDoorDirection(Door.rotateDoorDirection(beamFrom, true), true)*/beamFrom == this.receiveBeamFrom) {
            this.beamed = true;
            this.effectableObj.GetComponent<Effectable>().onEffect();
            this.beamedSprite.SetActive(true);
            this.notBeamedSprite.SetActive(false);

            if(this.puzzle != null) {
                this.puzzle.onActive(this.id);
            }
        }
    }

    public void deactivate() {
        this.beamed = false;
        this.beamedSprite.SetActive(false);
        this.notBeamedSprite.SetActive(true);
        if(this.puzzle != null) {
            this.puzzle.onDeactivate(this.id);
        }
        
        this.effectableObj.GetComponent<Effectable>().onEffectOver();
    }

    public void rotate90(bool clockwise) {
        this.receiveBeamFrom = Door.rotateDoorDirection(this.receiveBeamFrom, clockwise);
    }

    //gets the incoming direction needed to active the sink
    public DoorDirection getIncomingDirectionToActivate() {
        // return Door.rotateDoorDirection(Door.rotateDoorDirection(this.receiveBeamFrom, true), true);
        return this.receiveBeamFrom;
    }

}
