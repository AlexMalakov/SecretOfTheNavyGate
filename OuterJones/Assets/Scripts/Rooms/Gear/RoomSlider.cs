using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSlider : Effectable
{
    [SerializeField] private Room originRoom;
    [SerializeField] private Player player;

    public void onEffect() {
        this.originRoom.getLayoutManager().slideRoomsAroundCenter(this.originRoom.getPosition(), this.player.getRotationDirection());
    }
}
