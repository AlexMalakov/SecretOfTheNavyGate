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
    [SerializeField] private LightSink sink;
    
    private LightSource source;

    [SerializeField] private List<LightPassage> passages;

    public override void init(RoomCoords position) {
        this.position = position;
        if((position.x + position.y) % 2 == 0) {
            this.roomLighting = lightLevel;
        } else {
            this.roomLighting = darkLevel;
        }

        foreach(LightPassage p in this.passages) {
            p.informLighting((position.x + position.y) % 2 == 0);
        }
    }


    public override Sprite getRoomSprite() {
        if(Mathf.Abs(this.roomLighting - lightLevel) < .001) {
            return this.roomSprite;
        }
        return this.darkSprite;
    }

    public override void receiveBeam(DoorDirection incomingDirection) {
        if(this.sink != null) {
            this.sink.activate();
        } else if(this.mirror == null) {
            base.receiveBeam(incomingDirection);
        } else {
            DoorDirection exitDirection = this.mirror.reflect(this.getEntrance(incomingDirection).getInverse());
            if(this.hasDoorDirection(exitDirection)) {

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.mirror.transform.position);

                BeamModel bb = BeamPool.getBeam();
                this.beams.Add(bb);

                bb.initBeam(
                    this.transform,
                    this.mirror.transform.position,
                    this.getPointInDirection(this.getEntrance(incomingDirection).getInverse()).position);
            }
        }
        
    }

    public override void removeBeam() {
        base.removeBeam();
        if(this.sink != null) {
            this.sink.deactivate();
        }
    }


    
    public override void rotate90(bool clockwise) {
        base.rotate90(clockwise);

        if(this.source != null) {
            this.source.rotate90(clockwise);
        }

        if(this.mirror != null) {
            this.mirror.rotate90();
        }
    }

    public void setSource(LightSource s) {
        this.source = s;
    }
}
