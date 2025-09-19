using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRoom : Room
{
    [SerializeField] private List<AlternatingSpitter> spitters;
    [SerializeField] private RotationListener rotListener;

    public override void onEnter(Door d) {
        base.onEnter(d);

        foreach(AlternatingSpitter s in this.spitters) {
            s.onPlayerEnter();
        }
    }

    public override void onEnter(UnderbellyStaircase u) {
        base.onEnter(u);

        foreach(AlternatingSpitter s in this.spitters) {
            s.onPlayerEnter();
        }
    }

    public override void onExit() {
        base.onExit(); 

        foreach(AlternatingSpitter s in this.spitters) {
            s.onPlayerExit();
        }
    }

    public override bool rotate90(bool clockwise) {
        if(this.rotListener != null) {
            this.rotListener.onRotation(clockwise);
        }

        return base.rotate90(clockwise);
    }
}
