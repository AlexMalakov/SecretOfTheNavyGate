using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] public DoorDirection northReflectsTo;

    public DoorDirection reflect(DoorDirection enter) {
        if(enter == DoorDirection.North) {
            return northReflectsTo;
        } else if(enter == DoorDirection.South) {
            if(northReflectsTo == DoorDirection.East) {
                return DoorDirection.West;
            }
            return DoorDirection.East;
        } else if(enter == northReflectsTo) {
            return DoorDirection.North;
        }
        return DoorDirection.South;
    }

    public void rotate90() {
        if(northReflectsTo == DoorDirection.West) {
            northReflectsTo = DoorDirection.East;
        } else {
            northReflectsTo = DoorDirection.West;
        }
    }
}
