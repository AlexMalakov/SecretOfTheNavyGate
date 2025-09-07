using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spins R1, then spins the room that is pointed to by what was the south door
public class RoomSpinner : MonoBehaviour, Effectable
{
    [SerializeField] private GearRoom room;
    [SerializeField] private Door spinThrough;

    [SerializeField] private Player player;
    [SerializeField] private WaterDetector detector;

    public void onEffect() {
        if(!this.detector.isFlooded()) {
            return;
        }
        if(room.getLayoutManager().getRoomAt(room.getPosition().getOffset(spinThrough.getDirection())) != null) {
            room.getLayoutManager().getRoomAt(room.getPosition().getOffset(spinThrough.getDirection())).rotate90(!player.getRotationDirection());
        }

        room.rotate90(player.getRotationDirection());
    }

    public void onEffectOver(){}
}
