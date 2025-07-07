using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager : MonoBehaviour
{
    private MonkeyStatue[] statues;
    private Dictionary<string, bool> statueVals;

    private bool solved;
    private int correct;

    private bool order = true;
    
    [SerializeField] private List<string> order;


    public void Awake() {
        this.statues = GetComponents<MonkeyStatue>();

        foreach(MonkeyStatue m in this.statues) {
            m.init(this);
            this.statueVals.Add(m.getOrderVal(), false);
        }

        this.correct = 0
    }

    public void onStatueActive() {
        if(correct)
    }

    public void onRoomExit() {
        if(this.solved) {
            return;
        }

        foreach(MonkeyStatue m in this.statues) {
            m.reset();
        }

        this.correct = 0;
    }

    public void notify(MonkeyStatue m, bool value) {
        this.statueVals[m.getOrderVal()] = value;

        this.updateCorrect();

        if(correct == this.order.Count) {
            solved = true;
            this.onSolved();
        }
    }

    private void updateCorrect() {
        if(correct < 0) {
            foreach(MonkeyStatue m in this.statues) {
                if(this.statueVals[m.getOrderVal()]) {
                    return;
                }
            }
            correct == 0;
            return;
        }

        if(!value) {
            correct = -1;
            return;
        }

        if(correct == 0) {
            if(m.getOrderVal() == this.order[0] || m.getOrderVal() == this.order[this.order.Count - 1]) {
                correct++;
                order = m.getOrderVal() == this.order[0];
            } else {
                correct = -1;
            }
        } else if(correct > 0) {
            if((order && m.getOrderVal() == this.order[correct])
                    || (!order && m.getOrderVal() == this.order[this.order.Count - 1 - correct])) {
                
                correct++;
            } else {
                correct = -1;
            }
        }
    }


    private void onSolved() {
        //TODO: do solved stuff
    }

    public bool isSolved() {
        return this.isSolved();
    }

    public void reset() {
        foreach(MonkeyStatue m in this.statues) {
            m.reset();
            this.statueVals[m.getOrderVal()] = false;
        }

        this.solved = false;
        this.correct = 0;
    }
}
