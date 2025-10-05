using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManager : MonoBehaviour, InputSubscriber
{
    private MonkeyStatue[] statues;
    private Dictionary<string, bool> statueVals;

    private bool solved;
    private int correct;

    private bool lisOrder = true;
    
    [SerializeField] private List<string> order;
    [SerializeField] private GameObject effectableObj;

    [SerializeField] private GameObject defaultSprite;
    [SerializeField] private GameObject successSprite;

    private PlayerIO inputManager;

    public void Awake() {
        this.inputManager = FindObjectOfType<PlayerIO>();
        this.statues = GetComponentsInChildren<MonkeyStatue>();
        this.statueVals = new Dictionary<string, bool>();

        this.defaultSprite.SetActive(true);
        this.successSprite.SetActive(false);

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
            foreach(MonkeyStatue monk in this.statues) {
                if(this.statueVals[monk.getOrderVal()]) {
                    return;
                }
            }

            correct = 0;
            return;
        }

        if(!value) {
            correct = -1;
            return;
        }

        if(correct == 0) {
            if(m.getOrderVal() == this.order[0] || m.getOrderVal() == this.order[this.order.Count - 1]) {
                correct++;
                lisOrder = m.getOrderVal() == this.order[0];
            } else {
                correct = -1;
            }
        } else if(correct > 0) {
            if((lisOrder && m.getOrderVal() == this.order[correct])
                    || (!lisOrder && m.getOrderVal() == this.order[this.order.Count - 1 - correct])) {
                
                correct++;
            } else {
                correct = -1;
            }
        }
    }


    private void onSolved() {
        this.defaultSprite.SetActive(false);
        this.successSprite.SetActive(true);
        effectableObj.GetComponent<Effectable>().onEffect();
    }

    public bool isSolved() {
        return this.solved;
    }


    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null && !this.solved) {
            inputManager.requestSpaceInput(this, this.transform, "reset statues");
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            inputManager.cancelInputRequest(this);
        }
    }

    public void onSpacePress() {
        foreach(MonkeyStatue stat in this.statues) {
            stat.reset();
            correct = 0;
            effectableObj.GetComponent<Effectable>().onEffectOver();
        }
    }
}
