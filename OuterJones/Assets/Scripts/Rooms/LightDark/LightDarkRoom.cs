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

    [SerializeField] private LightSource source; //this is bad and needs to be moved somewhere else

    [SerializeField] private List<BeamModel> beams = new List<BeamModel>();

    [SerializeField] private List<LightPassage> passages;


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

                BeamModel b = this.source.getBeam();
                this.beams.Add(b);
                b.initBeam(
                    this.getPointInDirection(incomingDirection).position,
                    this.getPointInDirection(this.getEntrance(incomingDirection).getInverse()).position);

            }
            
        } else {
            DoorDirection exitDirection = this.mirror.reflect(this.getEntrance(incomingDirection).getInverse());
            if(this.hasDoorDirection(exitDirection)) {

                BeamModel b = this.source.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.getPointInDirection(incomingDirection).position,
                    this.mirror.transform.position);

                BeamModel bb = this.source.getBeam();
                this.beams.Add(bb);

                bb.initBeam(
                    this.mirror.transform.position),
                    this.getPointInDirection(this.getEntrance(incomingDirection).getInverse()).position);
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
        for(int i = 0; i < this.beams.Count; i++) {
            Destroy(this.beams[i].gameObject);
        }

        this.beams = new List<BeamModel>();
    }

}
