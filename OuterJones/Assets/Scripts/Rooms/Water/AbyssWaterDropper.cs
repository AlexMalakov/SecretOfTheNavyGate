using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//so this is basically just a grate but i dont want it to be cuz im worrioed about side affects
public class AbyssWaterDropper : Floodable
{
    [SerializeField] Canal destination;


    public override void onFlood() {
        this.destination.onFlood(null);
    }

    public override void drainWater() {
        
    }
}
