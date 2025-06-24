using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
big idea of code: each room has 16 possible entrances/exits, which is how cross room water flow is solved. If a canal in a room is flooded
and it exits into an adjacent room which has a canal entrance that "lines up", that canal should also flood

canals are made using tile maps. One tilemap for it's body, and a second to create a collider around it for when you fall in.
*/

public enum CanalEntrances {
    NW = 0, N = 1, NE = 2, E = 3, SE = 4, S = 5, SW = 6, W = 7
}


public class WaterRoom : Room
{
    //tbh not the greatest solution but should do the job
    [SerializeField] private List<CanalEntrances> canalEntrances;
    [SerializeField] private List<Canal> canals;

    public static int CANAL_ENTRANCE_COUNT = 8;

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

    public override void rotate90(bool clockwise) {
        base.rotate90(clockwise);

        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % CANAL_ENTRANCE_COUNT);
        }
        foreach(Canal c in this.canals) {
            c.rotate90(clockwise);
        }
    }

}
