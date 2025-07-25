using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzleButton : RotationPuzzleElement, InputSubscriber
{

    private RotationPuzzleManager manager;
    private int pressNum;

    [SerializeField] private GameObject pressedObj;
    [SerializeField] private GameObject pressableObj;
    [SerializeField] private GameObject notPressedObj;

    private PlayerInput input;

    public void Awake() {
        this.input = FindObjectOfType<PlayerInput>();
    }

    public void initButton(RotationPuzzleManager manager, int pressNum) {
        this.manager = manager;
        this.pressNum = pressNum;
    }

    public override void resetElement() {
        pressedObj.SetActive(false);
        pressableObj.SetActive(false);
        notPressedObj.SetActive(true);
    }

    public void readyToPress() {
        pressedObj.SetActive(false);
        pressableObj.SetActive(true);
        notPressedObj.SetActive(false);
    }

    public void isPressed() {
        pressedObj.SetActive(true);
        notPressedObj.SetActive(false);
        pressableObj.SetActive(false);
    }

    public int getButtonNum() {
        return this.pressNum;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(this.playerInCanal) {
            return;
        }

        if(other.GetComponent<Player>() != null) {
            this.input.requestSpaceInput(this, this.transform, "press button");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.input.cancelSpaceInputRequest(this);
        }
    }

    public override void onPlayerInCanal() {
        base.onPlayerInCanal();
        this.input.cancelSpaceInputRequest(this);
    }

    public void onSpacePress() {
        this.manager.onButtonPress(this);
    }
}
