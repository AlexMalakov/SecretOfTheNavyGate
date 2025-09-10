using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSlider : MonoBehaviour, Effectable
{
    [SerializeField] private Room originRoom;
    [SerializeField] private Player player;

    public void onEffect() {
        this.originRoom.getLayoutManager().slideRoomsAroundCenter(this.originRoom.getPosition(), this.player.getRotationDirection());
    }

    public void onEffectOver() {} //doesn't do anything
}
