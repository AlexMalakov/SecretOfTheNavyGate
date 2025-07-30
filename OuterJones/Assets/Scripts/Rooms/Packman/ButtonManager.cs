using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private Button[] buttons;
    private PowerableObject[] powerables;

    [SerializeField] private List<string> damSequence;
    [SerializeField] private List<string> puzzleSequence;

    [SerializeField] private List<GameObject> damEffectableTargets;
    [SerializeField] private List<GameObject> puzzleEffectableTargets;

    private int sequencePos;
    private bool isDamSequence;


    public void Awake() {
        this.buttons = GetComponentsInChildren<Button>();
        this.powerables = GetComponentsInChildren<PowerableObject>();
        this.sequencePos = 0;
    }

    public void init() {
        foreach(Button b in this.buttons) {
            b.init(this);
        }
    }


    public void onBottonPress(string buttonStr) {
        if(sequencePos == 0 && buttonStr == damSequence[0]) {
            isDamSequence = true;
        } else if(sequencePos == 0 && buttonStr == puzzleSequence[0]) {
            isDamSequence = false;
        }
        
        if((isDamSequence && buttonStr == puzzleSequence[sequencePos]) || (!isDamSequence && buttonStr == puzzleSequence[sequencePos])) {
            sequencePos++;
        } else {
            sequencePos = 0;
            //display fail
            return;
        }

        if(isDamSequence && sequencePos == damSequence.Count) {
            foreach(GameObject obj in this.damEffectableTargets) {
                obj.GetComponent<Effectable>().onEffect();
            }
        } else if(!isDamSequence && sequencePos == puzzleSequence.Count) {
            foreach(GameObject obj in this.puzzleEffectableTargets) {
                obj.GetComponent<Effectable>().onEffect();
            }
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