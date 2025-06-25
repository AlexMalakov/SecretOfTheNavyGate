using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanRoom : Room
{
    [SerializeField] private Mummy mummy;
    private Button[] buttons;

    public void Start() {
        this.buttons = GetComponentsInChildren<Button>();
    }

    public override void init(RoomCoords position) {
        base.init(position);

        foreach(Door d in this.doors) {
            if(d.getDestination() == null 
                && this.layoutManager.getRoomFromPackman(this.position.getOffset(d.getDirection())) != null
                && this.layoutManager.getRoomFromPackman(this.position.getOffset(d.getDirection())).hasDoorDirection(d.getInverse())) {

                
                d.setDestination(this.layoutManager.getRoomFromPackman(this.position.getOffset(d.getDirection())).getEntrance(d.getInverse()));
            }
        }
    }

    public override void onEnter() {
        base.onEnter();
        mummy.wake();
    }

    public override void onExit() {
        mummy.sleep();
        base.onExit();
    }


    public static bool isPackmanPlace(Door origin, int maxX, int maxY) {
        //i think this is fine to use old getOffset since the direction is important to identify if it's a packman
        return (origin.getDirection() == DoorDirection.North  && origin.getPosition().getOffset(0, 1).y == maxY)
                || (origin.getDirection() == DoorDirection.East && origin.getPosition().getOffset(1, 0).x == maxX)
                || (origin.getDirection() == DoorDirection.West && origin.getPosition().getOffset(-1, 0).x == 0)
                || (origin.getDirection() == DoorDirection.South && origin.getPosition().getOffset(0, -1).y == 0);
    }

    public void onButtonEvent(Button b, bool isPressed) {

    }



    public override void beamNeighbor(DoorDirection exitDirection) {
        if(this.layoutManager.getRoomFromPackman(this.position.getOffset(exitDirection).x, this.position.getOffset(exitDirection).y) != null) {
            this.layoutManager.getRoomFromPackman(this.position.getOffset(exitDirection).x, this.position.getOffset(exitDirection).y).receiveBeam(exitDirection);
        }
    }

    public override void floodNeighbors(List<CanalEntrances> exits) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomFromPackman(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1]) != null) {
                this.layoutManager.getRoomFromPackman(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1]).onFlood(exit);
            }
        }
    }
}
