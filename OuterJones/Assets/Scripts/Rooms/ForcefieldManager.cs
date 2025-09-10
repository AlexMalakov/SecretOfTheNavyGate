using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType {
    water, gear, packman, lightDark
}
public class ForcefieldManager : MonoBehaviour
{
    [SerializeField] List<Room> waterRooms;
    [SerializeField] List<Room> gearRooms;
    [SerializeField] List<Room> packmanRooms;
    [SerializeField] List<Room> lightDarkRooms;

    public void Awake() {
        this.controlForceFields(waterRooms, true);
        this.controlForceFields(gearRooms, true);
        this.controlForceFields(packmanRooms, true);
        this.controlForceFields(lightDarkRooms, true);
    }


    public void deactivateForceField(RoomType type) {
        switch(type) {
            case RoomType.water:
                this.controlForceFields(this.waterRooms, false);
                break;
            case RoomType.gear:
                this.controlForceFields(this.gearRooms, false);
                break;
            case RoomType.packman:
                this.controlForceFields(this.packmanRooms, false);
                break;
            case RoomType.lightDark:
                this.controlForceFields(this.lightDarkRooms, false);
                break;
        }
    }

    public void controlForceFields(List<Room> rooms, bool active) {
        foreach(Room r in rooms) {
            foreach(Door d in r.getDoors()) {
                d.setForceField(active);
            }
        }
    }
}
