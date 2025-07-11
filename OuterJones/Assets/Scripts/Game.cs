using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] RoomsLayout layout;
    [SerializeField] Player player;

    public void resetGame() {
        //TODO: maybe the sfcene can reset itself quickly?
        foreach(Room r in this.layout.getAllRooms()) {
            r.resetRoom();
        }

        this.layout.reset();

        //reset all UI elements

        //reset player, contorller and inventory
        player.resetPlayer();

    }
}


public interface Effectable {
    void onEffect();

    void reset();
}