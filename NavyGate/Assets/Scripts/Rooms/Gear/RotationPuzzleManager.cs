using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotationPuzzleManager : Floodable, Effectable
{
    [SerializeField] private List<GameObject> puzzleElementObjects;
    [SerializeField] private List<RotationPuzzleButton> rButtons;
    private int buttonInOrder = 0;

    [SerializeField] private PowerableDoor puzzleEntrance;
    [SerializeField] private PowerableDoor puzzleFinish;

    private List<RotationPuzzleElement> puzzleElements;
    private bool flooded = false;
    private bool solved = false;

    void Awake() {
        puzzleElements = new List<RotationPuzzleElement>();

        foreach(GameObject obj in this.puzzleElementObjects) {
            this.puzzleElements.Add(obj.GetComponent<RotationPuzzleElement>());
        }

        foreach(RotationPuzzleElement r in this.puzzleElements) {
            r.init();
        }

        for(int i = 0; i < rButtons.Count; i++) {
            rButtons[i].initButton(this, i);
        }
    }

    public void resetPuzzle() {
        this.solved = false;
        foreach(RotationPuzzleElement e in this.puzzleElements) {
            e.resetElement();
        }

        buttonInOrder = 0;
        this.rButtons[0].readyToPress();
    }

    public void onButtonPress(RotationPuzzleButton button) {
        if(button.getButtonNum() == buttonInOrder) {
            button.isPressed();
            buttonInOrder++;

            if(buttonInOrder >= rButtons.Count) {
                this.solved = true;
                puzzleFinish.opencloseDoor(true);
                return;
            }

            this.rButtons[buttonInOrder].readyToPress();
        }
    }

    public void onPlayerInCanal() {
        foreach(RotationPuzzleElement r in this.puzzleElements) {
            r.onPlayerInCanal();
        }
    }

    public void onPlayerOutCanal() {
        foreach(RotationPuzzleElement r in this.puzzleElements) {
            r.onPlayerOutCanal();
        }
    }

    public void onEffect() {
        this.resetPuzzle();
        if(!this.flooded) {
            puzzleEntrance.opencloseDoor(true);
        }
    }

    public void onEffectOver(){}

    void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            puzzleEntrance.opencloseDoor(false);
            puzzleFinish.opencloseDoor(this.solved);
        }
    }

    public override void onFlood(bool fromSource) {
        this.flooded = true;
        puzzleEntrance.opencloseDoor(false);
        puzzleFinish.opencloseDoor(false);
    }

    public override void drainWater() {
        this.flooded = false;
    }
}
