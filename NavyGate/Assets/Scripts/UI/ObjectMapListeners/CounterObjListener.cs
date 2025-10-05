using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterObjListener : ObjListener
{
    [SerializeField] private List<Sprite> activatedSprites;

    public void activateNumber(int activate) {
        if(activate >= 0) {
            this.activatedSprite = this.activatedSprites[activate];
            this.onStatusChanged(true);
        }
    }
}
