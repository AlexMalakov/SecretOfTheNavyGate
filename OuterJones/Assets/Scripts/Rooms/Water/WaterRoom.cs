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

    private Dictionary<CanalEntrances, int[]> neighborMap = new Dictionary<CanalEntrances, int[]>() {
        {CanalEntrances.NW, new int[]{-1, 1}},
        {CanalEntrances.N, new int[]{0, 1}},
        {CanalEntrances.NE, new int[]{1, 1}},
        {CanalEntrances.E, new int[]{1, 0}},
        {CanalEntrances.SE, new int[]{1, -1}},
        {CanalEntrances.S, new int[]{0, -1}},
        {CanalEntrances.SW, new int[]{-1, -1}},
        {CanalEntrances.W, new int[]{-1, 0}},
    }

    public void rotate(int turns) {
        //rotate which canal entrances are open/closed
    }


    public override void floodNeighbors(List<CanalEntrances> exits) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomAt(this.position.x + neighborMap[exit].x, this.position.y + neighorMap[exit].y) != null) {
                this.layoutManager.getRoomAt(this.position.x + neighborMap[exit].x, this.position.y + neighorMap[exit].y).onFlood(exit);
            }
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
