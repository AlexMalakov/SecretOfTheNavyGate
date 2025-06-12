using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool flooded = false;

    

    public void rotate(int turns) {
        //rotate which canal entrances are open/closed
    }

    public void updateFlood(List<CanalEntrances> floodingFrom) {
        if(flooded) {
            return;
        }
        flooded = true;
        List<CanalEntrances> floodTo = new List<CanalEntrances>();
        foreach(Canal c in this.canals) {
            if(c.willFlood(floodingFrom)) {
                floodTo.AddRange(c.onFlood(floodingFrom));
            }
        }

        passFloodOn(floodTo);
    }

    private void passFloodOn(List<CanalEntrances> exits) {
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
            this.layoutManager.getRoomAt(this.position.x, this.position.y+1).updateFlood(northExits);
        }
        if(eastExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x+1, this.position.y).updateFlood(eastExits);
        }
        if(southExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x, this.position.y-1).updateFlood(southExits);
        }
        if(westExits.Count > 0) {
            this.layoutManager.getRoomAt(this.position.x-1, this.position.y+1).updateFlood(westExits);
        }
        
    }

}
