using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRoom : Room
{
    [SerializeField] private List<AlternatingSpitter> spitters;
    [SerializeField] private RotationListener rotListener;
    [SerializeField] private AlternatingSpitterListener spitListener;

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
        bool b = base.rotate90(clockwise);
        if(this.rotListener != null) {
            this.rotListener.onRotation(clockwise);
        }

        if(this.spitListener != null) {
            this.spitListener.rotate90();
        }

        return b;
    }
}
