using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAlternatingSpitter : AlternatingSpitter, RotationPuzzleElement, InputSubscriber
{

    private PlayerInput input;
    private Quaternion initialRot;

    private PlayerController controller;

    protected void Start() {
        this.input = FindObjectOfType<PlayerInput>();
        this.controller = FindObjectOfType<PlayerController>();

        this.initialRot = this.transform.rotation;
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
        this.transform.rotation = initialRot;
    }

    public void onSpacePress() {
        StartCoroutine(rotateSpitter(this.controller));
    }
}
