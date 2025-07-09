using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Floodable, InputSubscriber
{
    [SerializeField] private Transform ladderExit;
    private Canal canal;

    private PlayerInput inputManager;
    private Player player;

    private bool flooded = false;

    public void Awake() {
        this.inputManager = FindObjectOfType<PlayerInput>();
        this.player = FindObjectOfType<Player>();
    }

    public void init(Canal c) {
        this.canal = c;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && !flooded) {
            inputManager.requestSpaceInput(this, this.transform, "climb ladder");
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            inputManager.cancelSpaceInputRequest(this);
        }
    }

    public void onSpacePress() {
        this.player.transform.position = ladderExit.position;
        this.canal.onLadderUse();
    }

    public override void onFlood() {
        this.flooded = true;
    }

    public override void drainWater() {
        this.flooded = false;
    }
}
