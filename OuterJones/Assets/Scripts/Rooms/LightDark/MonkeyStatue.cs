using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyStatue : MonoBehaviour, InputSubscriber
{

    private bool state = false;
    [SerializeField] GameObject onModel;
    [SerializeField] GameObject offModel;
    [SerializeField] private string orderIdentifier;

    private StatueManager manager;
    private PlayerIO inputManager;


    public void Awake() {
        this.inputManager = FindObjectOfType<PlayerIO>();
    }

    public void init(StatueManager manager) {
        this.manager = manager;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null && !this.manager.isSolved()) {
            inputManager.requestSpaceInput(this, this.transform, ((this.state) ? "deactivate statue" : "activate statue"));
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            inputManager.cancelInputRequest(this);
        }
    }

    public void onSpacePress() {
        if(!this.manager.isSolved()) {
            this.toggleState();

            this.manager.notify(this, this.state);
            // inputManager.requestSpaceInput(this, this.transform, ((this.state) ? "deactivate statue" : "activate statue"));
        }
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
