using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager : MonoBehaviour
{
    private MonkeyStatue[] statues;
    private Dictionary<string, bool> statueVals;

    private bool solved;
    private int correct;

    private bool lisOrder = true;
    
    [SerializeField] private List<string> order;
    [SerializeField] private GameObject effectableObj;


    public void Awake() {
        this.statues = GetComponentsInChildren<MonkeyStatue>();
        this.statueVals = new Dictionary<string, bool>();

        foreach(MonkeyStatue m in this.statues) {
            m.init(this);
            this.statueVals.Add(m.getOrderVal(), false);
        }

        this.correct = 0;
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

        this.updateCorrect(m, value);

        if(correct == this.order.Count) {
            solved = true;
            this.onSolved();
        }
    }

    private void updateCorrect(MonkeyStatue m, bool value) {
        if(correct < 0) {
            Debug.Log("?????");
            foreach(MonkeyStatue monk in this.statues) {
                if(this.statueVals[monk.getOrderVal()]) {
                    return;
                }
            }

            correct = 0;
            return;
        }

        if(!value) {
            Debug.Log("erm what the flip");
            correct = -1;
            return;
        }

        if(correct == 0) {
            if(m.getOrderVal() == this.order[0] || m.getOrderVal() == this.order[this.order.Count - 1]) {
                correct++;
                Debug.Log("+111111");
                lisOrder = m.getOrderVal() == this.order[0];
            } else {
                Debug.Log("failed :(");
                correct = -1;
            }
        } else if(correct > 0) {
            if((lisOrder && m.getOrderVal() == this.order[correct])
                    || (!lisOrder && m.getOrderVal() == this.order[this.order.Count - 1 - correct])) {
                
                correct++;
                Debug.Log("+1!!");
            } else {
                Debug.Log("FAILED!");
                correct = -1;
            }
        }
    }


    private void onSolved() {
        Debug.Log("SOLVED!");
        effectableObj.GetComponent<Effectable>().onEffect();
    }

    public bool isSolved() {
        return this.solved;
    }
}
