using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDarkRoom : Room
{
    [Header ("lightdark")]
    [SerializeField] private float lightLevel;
    [SerializeField] private float darkLevel;

    [SerializeField] private Sprite darkSprite;
    [SerializeField] private Mirror mirror;

    [SerializeField] private List<BeamModel> beams = new List<BeamModel>();


    public override void init(RoomCoords position) {
        this.position = position;
        if((position.x + position.y) % 2 == 0) {
            this.roomLighting = lightLevel;
        } else {
            this.roomLighting = darkLevel;
        }
    }


    public override Sprite getRoomSprite() {
        if(Mathf.Abs(this.roomLighting - lightLevel) < .001) {
            return this.roomSprite;
        }
        return this.darkSprite;
    }

    public override void receiveBeam(DoorDirection incomingDirection) {
        if(this.mirror == null) {
            if(this.hasDoorDirection(this.getEntrance(incomingDirection).getInverse())) {
                //send it out of the exit
            }
            
        } else {
            DoorDirection exitDirection = this.mirror.reflect(this.getEntrance(incomingDirection).getInverse());
            if(this.hasDoorDirection(exitDirection)) {
                //send it out the exit
            }
        }
        
    }

    public override void beamNeighbor(DoorDirection exitDirection) {
        RoomCoords exit;

        switch(exitDirection) {
            case DoorDirection.North:
                exit = new RoomCoords(this.position.x, this.position.y+1);
                break;
            case DoorDirection.East:
                exit = new RoomCoords(this.position.x+1, this.position.y);
                break;
            case DoorDirection.South:
                exit = new RoomCoords(this.position.x, this.position.y-1);
                break;
            case DoorDirection.West:
                exit = new RoomCoords(this.position.x-1, this.position.y);
                break;
            default:
                Debug.Log("IMPOSSIBLE COORDINATE");
                return;
        }

        this.layoutManager.getRoomAt(exit.x, exit.y).receiveBeam(exitDirection);
    }

    public override void removeBeam() {
        //remove line object that represents the beam
    }

}
