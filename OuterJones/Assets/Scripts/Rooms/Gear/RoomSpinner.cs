using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spins R1, then spins the room that is pointed to by what was the south door
public class RoomSpinner : MonoBehaviour
{
    [SerializeField] private GearRoom room;
    [SerializeField] private Door spinThrough;

    public void onActivate(bool clockwise) {
        if(room.getLayoutManager().getRoomAt(room.getPosition().getOffset(spinThrough.getDirection())) != null) {
            room.getLayoutManager().getRoomAt(room.getPosition().getOffset(spinThrough.getDirection())).rotate90(!clockwise);
        }

        room.rotate90(clockwise);
    }
}
