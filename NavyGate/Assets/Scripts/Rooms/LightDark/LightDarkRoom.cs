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

    [SerializeField] private LDAlternatePuzzle ldListener;
    [SerializeField] private LightPuzzleRoom ldID;
    
    


    public override void init(RoomCoords position) {
        base.init(position);

        this.position = position;
        if((position.x + position.y) % 2 == ((position.overworld) ? 1 : 0)) {
            this.roomLighting = lightLevel;

            if(this.ldListener != null) {
                ldListener.informLight(true, this.ldID);
            }
        } else {
            this.roomLighting = darkLevel;

            if(this.ldListener != null) {
                ldListener.informLight(false, this.ldID);
            }
        }
    }

    public override bool canCastBeam() {
        return this.isLight();
    }

    public override Sprite getRoomSprite() {
        if(isLight()) {
            return this.roomSprite;
        }
        return this.darkSprite;
    }

    public Sprite getPlacementSprite(RoomCoords position) {
        if((position.x + position.y) % 2 == ((position.overworld) ? 1 : 0)) {
            return this.darkSprite;
        }
        return this.roomSprite;
    }
    
    public bool isLight() {
        return Mathf.Abs(this.roomLighting - lightLevel) < .001;
    }
}
