using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private Button[] buttons;
    private PowerableObject[] powerables;

    private bool startedSequence;


    public void Awake() {
        this.buttons = GetComponentsInChildren<Button>();
        this.powerables = GetComponentsInChildren<PowerableObject>();
        this.startedSequence = false;
    }

    public void init() {
        foreach(Button b in this.buttons) {
            b.init(this);
        }
    }


    public bool canStartSequence(Button b) {
        if(b.isStartingButton() || !startedSequence) {
            this.startedSequence = true;
            return true;
        } else {
            foreach(PowerableObject p in this.powerables) {
                p.reset();
            }
            return false;
        }
    }
}

public interface PowerableObject {
    void onPowered();
    void reset();
}