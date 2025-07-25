using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRoom : Room
{
    [SerializeField] private List<AlternatingSpitter> spitters;

    public override void onEnter() {
        base.onEnter();

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
}
