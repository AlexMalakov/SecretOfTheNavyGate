using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
big idea of code: each room has 8 possible entrances/exits, which is how cross room water flow is solved. If a canal in a room is flooded
and it exits into an adjacent room which has a canal entrance that "lines up", that canal should also flood

canals are made using tile maps. One tilemap for it's body, and a second to create a collider around it for when you fall in.
*/

public enum CanalEntrances {
    NW = 0, N = 1, NE = 2, E = 3, SE = 4, S = 5, SW = 6, W = 7
}


//this class no longer has any unique implementations. This is due to all room types possibly having canals, so nothing
//makes this room type unique. This is ok since it still defines important canal behaviors
public class WaterRoom : Room
{
    public static int CANAL_ENTRANCE_COUNT = 8;

    public static Dictionary<CanalEntrances, int[]> CANAL_N_MAP = new Dictionary<CanalEntrances, int[]>() {
        {CanalEntrances.NW, new int[]{-1, 1}},
        {CanalEntrances.N, new int[]{0, 1}},
        {CanalEntrances.NE, new int[]{1, 1}},
        {CanalEntrances.E, new int[]{1, 0}},
        {CanalEntrances.SE, new int[]{1, -1}},
        {CanalEntrances.S, new int[]{0, -1}},
        {CanalEntrances.SW, new int[]{-1, -1}},
        {CanalEntrances.W, new int[]{-1, 0}},
    };
}
