using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRoom : Room
{
    [SerializeField] private AlternatingSpitter spitter;

    public override void onEnter() {
        base.onEnter();

        if(this.spitter != null) {
            this.spitter.onPlayerEnter();
        }
    }

    public override void onExit() {
        if(spitter != null) {
            this.spitter.onPlayerExit();
        }
        this.gameObject.SetActive(false);
    }
}
