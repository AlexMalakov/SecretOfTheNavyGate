using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotatingGear : RotatingGear, RotationPuzzleElement, InputSubscriber
{
    private PlayerInput input;
    private Quaternion initialRot;


    public void Start() {
        base.Start();
        this.input = FindObjectOfType<PlayerInput>();

        this.initialRot = this.transform.rotation;

    }
    public override void playerOnTooth(GearTooth t, PlayerController controller) {

        this.input.requestSpaceInput(this, this.transform, "rotate platform");
    }

    public override void playerOffTooth() {
        this.input.cancelSpaceInputRequest(this);
    }

    public void onSpacePress() {
        StartCoroutine(this.rotateGear(controller));
    }

    public void resetElement() {
        this.transform.rotation = this.initialRot;
    }
}
