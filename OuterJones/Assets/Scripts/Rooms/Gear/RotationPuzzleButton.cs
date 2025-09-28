using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzleButton : RotationPuzzleElement, InputSubscriber
{

    private RotationPuzzleManager manager;
    private int pressNum;
    private bool isPressable;

    [SerializeField] private GameObject pressedObj;
    [SerializeField] private GameObject pressableObj;
    [SerializeField] private GameObject notPressedObj;

    private PlayerIO input;
    private Quaternion initialRott;

    public void Awake() {
        this.input = FindObjectOfType<PlayerIO>();
        this.initialRott = this.transform.rotation;
    }

    public void initButton(RotationPuzzleManager manager, int pressNum) {
        this.manager = manager;
        this.pressNum = pressNum;
    }

    public override void resetElement() {
        pressedObj.SetActive(false);
        pressableObj.SetActive(false);
        notPressedObj.SetActive(true);
        this.isPressable = false;
    }

    public void readyToPress() {
        pressedObj.SetActive(false);
        pressableObj.SetActive(true);
        notPressedObj.SetActive(false);
        this.isPressable = true;
    }

    public void isPressed() {
        pressedObj.SetActive(true);
        notPressedObj.SetActive(false);
        pressableObj.SetActive(false);
        this.isPressable = false;
    }

    public int getButtonNum() {
        return this.pressNum;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(this.playerInCanal) {
            return;
        }

        if(other.GetComponent<Player>() != null && this.isPressable) {
            this.input.requestSpaceInput(this, this.transform, "press button");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.input.cancelInputRequest(this);
        }
    }

    public void rotate90() {
        this.transform.rotation = this.initialRott;
    }

    public override void onPlayerInCanal() {
        base.onPlayerInCanal();
        this.input.cancelInputRequest(this);
    }

    public void onSpacePress() {
        this.manager.onButtonPress(this);
    }
}
