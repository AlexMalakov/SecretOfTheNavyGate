using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotatingGear : RotatingGear, RotationPuzzleElement, InputSubscriber
{
    private PlayerInput input;
    private Quaternion initialRot;
    private PlayerController controller;
    private GearTooth recent;

    [SerializeField] bool oneWay;

    protected override void Start() {
        base.Start();
        this.input = FindObjectOfType<PlayerInput>();

        this.initialRot = this.transform.rotation;
    }

    public override void playerOnTooth(GearTooth t, PlayerController controller) {
        this.controller = controller;
        this.recent = t;
        this.input.requestSpaceInput(this, this.transform, "rotate platform");
    }

    public override void playerOffTooth() {
        this.input.cancelSpaceInputRequest(this);
    }

    public void onSpacePress() {
        if(oneWay) {
            base.playerOnTooth(this.recent, this.controller);
        } else {
            StartCoroutine(this.rotateGear(this.controller));
        }

    }

    public void resetElement() {
        this.transform.rotation = this.initialRot;
    }
}
