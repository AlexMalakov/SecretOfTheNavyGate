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
    private PlayerInput inputManager;


    public void Awake() {
        this.inputManager = FindObjectOfType<PlayerInput>();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null && PlayerInput.getSpaceInput(this.transform, "activate statue") && !this.manager.isSolved()) {

            this.toggleState();

            this.manager.notify(this, this.state);
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            inputManager.cancelSpaceInputRequest(this);
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
