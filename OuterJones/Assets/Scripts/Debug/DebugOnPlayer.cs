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
    }
}
