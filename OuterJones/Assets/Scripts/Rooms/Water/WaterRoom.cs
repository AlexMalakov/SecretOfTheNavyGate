using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
big idea of code: each room has 16 possible entrances/exits, which is how cross room water flow is solved. If a canal in a room is flooded
and it exits into an adjacent room which has a canal entrance that "lines up", that canal should also flood

canals are made using tile maps. One tilemap for it's body, and a second to create a collider around it for when you fall in.
*/

public enum CanalEntrances {
    northFL = 0, northCL = 1, northCR= 2, northFR = 3,
    eastFT = 4, eastCT = 5, eastCB = 6, eastFB = 7,
    southFR = 8, southCR = 9, southCL = 10, southFL = 11,
    westFB = 12, westCB = 13, westCT = 14, westFT = 15,
}
public class WaterRoom : Room
{
    //tbh not the greatest solution but should do the job
    [SerializeField] private List<CanalEntrances> canalEntrances;
    [SerializeField] private List<Canal> canals;

    public void rotate(int turns) {
        //rotate which canal entrances are open/closed
    }


    public override void floodNeighbors(List<CanalEntrances> exits) {
        List<CanalEntrances> northExits = new List<CanalEntrances>();
        List<CanalEntrances> eastExits = new List<CanalEntrances>();
        List<CanalEntrances> southExits = new List<CanalEntrances>();
        List<CanalEntrances> westExits = new List<CanalEntrances>();
        foreach(CanalEntrances c in exits) {
            switch(((int)c)/4) {
                case 0:
                    northExits.Add(c);
                    break;
                case 1:
                    eastExits.Add(c);
                    break;
                case 2:
                    southExits.Add(c);
                    break;
                case 3:
                    westExits.Add(c);
                    break;
            }
        }

        if(northExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x, this.position.y+1).onFlood(northExits);
        }
        if(eastExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x+1, this.position.y).onFlood(eastExits);
        }
        if(southExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x, this.position.y-1).onFlood(southExits);
        }
        if(westExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x-1, this.position.y+1).onFlood(westExits);
        }
    }

    public override void onFlood(List<CanalEntrances> floodingFrom) {
        foreach(Canal c in this.canals) {
            if(c.willFlood(floodingFrom)) {
                c.onFlood(floodingFrom);
            }
        }
    }

    public override void drainWater() {
        foreach(Canal c in this.canals) {
            c.drainWater();
        }
    }

}
