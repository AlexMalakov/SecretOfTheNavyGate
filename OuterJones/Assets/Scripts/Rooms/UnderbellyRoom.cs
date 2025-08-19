using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderbellyRoom : Room
{
    [Header ("underbelly fields")]
    [SerializeField] private Room overworldPair;

    public override UnderbellyRoom getUnderbellyPair() {
        return null;//or this?
    }

    public Room getOverworldRoom() {
        return this.overworldPair;
    }
}
