using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAlternatingSpitter : AlternatingSpitter, RotationPuzzleElement
{

    private PlayerInput input;
    private Quaternion initialRot;

    private PlayerController controller;

    protected override void Start() {
        base.Start();
        this.input = FindObjectOfType<PlayerInput>();

        this.initialRot = this.transform.rotation;
    }

    protected override void activateAlternatingSpitter() {
        this.input.requestSpaceInput(this, this.transform, "rotate spinner");
    }

    private void OnTriggerExit2D(Collider2D other) {
        this.input.cancelSpaceInput(this);
    }

    public void resetElement() {
        this.transform.rotation = initialRot;
    }

    public void onSpacePress() {
        StartCoroutine(rotateSpitter(this.controller));
    }
}
