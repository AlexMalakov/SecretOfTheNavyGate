using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyStatue : MonoBehaviour
{

    private bool state = false;
    [SerializeField] GameObject onModel;
    [SerializeField] GameObject offModel;
    [SerializeField] private string orderIdentifier;

    private StatueManager manager;


    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null && PlayerInput.getSpaceInput(this.transform) && !this.manager.isSolved()) {
            this.toggleState();

            this.manager.notify(this, this.state);
        }
    }

    public void init(StatueManager manager) {
        this.manager = manager;
    }


    public void reset() {
        this.state = false;
        this.offModel.SetActive(true);
        this.onModel.SetActive(false);
    }

    private void toggleState() {
        this.state = !this.state;
        this.offModel.SetActive(!this.state);
        this.onModel.SetActive(this.state);
    }

    public string getOrderVal() {
        return this.orderIdentifier;
    }
}
