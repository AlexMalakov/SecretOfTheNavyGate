using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAlternatingSpitter : AlternatingSpitter, RotationPuzzleElement, InputSubscriber
{

    private PlayerInput input;
    private Quaternion initialRot;

    private PlayerController controller;

    private bool startDirection;

    protected void Start() {
        this.input = FindObjectOfType<PlayerInput>();
        this.controller = FindObjectOfType<PlayerController>();

        this.initialRot = this.transform.rotation;

        this.startDirection = this.clockwise;
    }

    protected override void activateAlternatingSpitter(PlayerController controller) {
        this.input.requestSpaceInput(this, this.transform, "rotate spinner");
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            this.input.cancelSpaceInputRequest(this);
        }
    }

    public void resetElement() {
        this.clockwise = this.startDirection;
    }

    public void onSpacePress() {
        StartCoroutine(rotateSpitter(this.controller));
    }
}
