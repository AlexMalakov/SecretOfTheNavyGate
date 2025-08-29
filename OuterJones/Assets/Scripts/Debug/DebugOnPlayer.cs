using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnPlayer : MonoBehaviour
{
    [SerializeField] private Player player;

    public void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            this.player.getCurrentRoom().rotate90(player.getRotationDirection());
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.NW);
        } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.N);
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.NE);
        } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.E);
        } else if(Input.GetKeyDown(KeyCode.Alpha5)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.SE);
        } else if(Input.GetKeyDown(KeyCode.Alpha6)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.S);
        } else if(Input.GetKeyDown(KeyCode.Alpha7)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.SW);
        } else if(Input.GetKeyDown(KeyCode.Alpha8)) {
            this.player.getCurrentRoom().onFlood(CanalEntrances.W);
        }

        if(Input.GetKeyDown(KeyCode.I)) {
            this.player.getCurrentRoom().receiveBeam(DoorDirection.North);
        } else if (Input.GetKeyDown(KeyCode.J)) {
            this.player.getCurrentRoom().receiveBeam(DoorDirection.West);
        } else if (Input.GetKeyDown(KeyCode.K)) {
            this.player.getCurrentRoom().receiveBeam(DoorDirection.South);
        } else if (Input.GetKeyDown(KeyCode.L)) {
            this.player.getCurrentRoom().receiveBeam(DoorDirection.East);
        }


        if(Input.GetKeyDown(KeyCode.Backspace)) {
            FindObjectOfType<WaterSourceManager>().restartFlood();
            FindObjectOfType<LightSourceManager>().resetBeams();
        }
    }
}
