using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private PowerableButton[] buttons;
    // private PowerableObject[] powerables;
    [Header("dam puzzle")]
    [SerializeField] private List<string> damSequence;
    [SerializeField] private List<Wire> damWires;
    [Header("wire puzzle")]
    [SerializeField] private List<string> puzzleSequence;
    [SerializeField] private List<Wire> puzzleWires;

    [SerializeField] private List<GameObject> damEffectableTargets;
    [SerializeField] private List<GameObject> puzzleEffectableTargets;

    private int sequencePos;
    private bool isDamSequence;

    private bool buttonsPressable = true;

    public void Awake() {
        this.buttons = GetComponentsInChildren<PowerableButton>();
        // this.powerables = GetComponentsInChildren<PowerableObject>();
        this.sequencePos = 0;
    }

    public void init() {
        foreach(PowerableButton b in this.buttons) {
            b.init(this);
        }
    }

    public int getSequencePos() {
        return this.sequencePos;
    }


    public void onBottonPress(string buttonStr) {
        if(!buttonsPressable) {
            this.failButtons();
            return;
        }

        if(sequencePos == 0 && buttonStr == damSequence[0]) {
            isDamSequence = true;
        } else if(sequencePos == 0 && buttonStr == puzzleSequence[0]) {
            isDamSequence = false;
        }
        
        if((!isDamSequence && buttonStr == puzzleSequence[sequencePos]) || (isDamSequence && buttonStr == damSequence[sequencePos])) {
            if(sequencePos < damWires.Count && isDamSequence) {
                StartCoroutine(damWires[sequencePos].wireAnimation(this));
            } else if(sequencePos < puzzleWires.Count && !isDamSequence) {
                StartCoroutine(puzzleWires[sequencePos].wireAnimation(this));
            }

            Debug.Log("SEQ PLUS PLUS");
            sequencePos++;
        } else {
            this.failButtons();
            return;
        }

        if((isDamSequence && sequencePos == damSequence.Count) || (!isDamSequence && sequencePos == puzzleSequence.Count)) {
            this.succeedButtons();
        }
    }  

    public void failButtons() {
        Debug.Log("FAILED");
        this.sequencePos = 0;
        foreach(PowerableButton b in this.buttons) {
            b.flashFailed();
        }
    }

    public void succeedButtons() {
        sequencePos = 0;
        foreach(GameObject obj in (isDamSequence? this.damEffectableTargets : this.puzzleEffectableTargets)) {
            obj.GetComponent<Effectable>().onEffect();
        }

        foreach(PowerableButton b in this.buttons) {
            b.flashSuccess();
        }
    }

    public void onWireFinished() {
        this.buttonsPressable = true;
        if(sequencePos > 0) {
            foreach(PowerableButton b in this.buttons) {
                b.flashPressable();
            }
        }
        //flash all buttons
    }
}