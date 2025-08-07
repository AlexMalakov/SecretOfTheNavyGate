using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanRoom : Room
{
    [SerializeField] private Mummy mummy;
    [SerializeField] private ButtonManager bManager;

    private Door enteredFrom;

    public override void init(RoomCoords position) {
        base.init(position);

        foreach(Door d in this.doors) {
            if(d.getDestination() == null 
                && this.layoutManager.getRoomFromPackman(this.position.getOffset(d.getDirection())) != null
                && this.layoutManager.getRoomFromPackman(this.position.getOffset(d.getDirection())).hasDoorDirection(d.getInverse())) {

                
                d.setDestination(this.layoutManager.getRoomFromPackman(this.position.getOffset(d.getDirection())).getEntrance(d.getInverse()));
            }
        }

        if(bManager != null) {
            bManager.init();
        }
    }

    public override void onEnter(Door d) {
        base.onEnter(d);
        if(mummy != null)
            mummy.wake();

        this.enteredFrom = d;
    }

    public override void onExit() {
        if(mummy != null)
            mummy.sleep();

        base.onExit();
    }

    public void resetPackmanRoom(Player p) {
        p.transform.postion = this.enteredFrom.getEnterPos();
        if(this.bManager != null) {
            this.bManager.failButtons();
        }
    }

    public static bool isPackmanPlace(Door origin, int maxX, int maxY) {
        //i think this is fine to use old getOffset since the direction is important to identify if it's a packman
        return (origin.getDirection() == DoorDirection.North  && origin.getPosition().getOffset(0, 1).y == maxY)
                || (origin.getDirection() == DoorDirection.East && origin.getPosition().getOffset(1, 0).x == maxX)
                || (origin.getDirection() == DoorDirection.West && origin.getPosition().getOffset(-1, 0).x == 0)
                || (origin.getDirection() == DoorDirection.South && origin.getPosition().getOffset(0, -1).y == 0);
    }

    public override void beamNeighbor(DoorDirection exitDirection) {
        if(this.layoutManager.getRoomFromPackman(this.position.getOffset(exitDirection).x, this.position.getOffset(exitDirection).y) != null) {
            this.layoutManager.getRoomFromPackman(this.position.getOffset(exitDirection).x, this.position.getOffset(exitDirection).y).receiveBeam(exitDirection);
        }
    }

    public override void floodNeighbors(List<CanalEntrances> exits) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomFromPackman(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1]) != null) {
                CanalEntrances opposite = (CanalEntrances)(((int)exit + (WaterSource.CANAL_ENTRANCE_COUNT/2)) % WaterSource.CANAL_ENTRANCE_COUNT);
                this.layoutManager.getRoomFromPackman(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1]).onFlood(opposite);
            }
        }
    }
}