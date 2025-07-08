using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDarkRoom : Room
{
    [Header ("lightdark")]
    [SerializeField] private float lightLevel;
    [SerializeField] private float darkLevel;
    [SerializeField] private StatueManager statueManager;

    [SerializeField] private Sprite darkSprite;
    
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

    public override void resetRoom() {
        base.resetRoom();

        if(this.statueManager != null) {
            this.statueManager.reset();
        }
    }

    public override Sprite getRoomSprite() {
        if(Mathf.Abs(this.roomLighting - lightLevel) < .001) {
            return this.roomSprite;
        }
        return this.darkSprite;
    }
    
    public override bool rotate90() {
        bool clockwise = base.rotate90();

        if(this.source != null) {
            this.source.rotate90(clockwise);
        }

        return clockwise;
    }

    public void setSource(LightSource s) {
        this.source = s;
    }
}
