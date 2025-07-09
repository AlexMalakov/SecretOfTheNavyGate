using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour, InputSubscriber
{
    [SerializeField] GameObject open;
    [SerializeField] GameObject closed;

    private PlayerInput inputManager;
    private Player player;

    private bool isOpen = false;

    public void Awake() {
        this.inputManager = FindObjectOfType<PlayerInput>();
        this.player = FindObjectOfType<Player>();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(isOpen) {
            return;
        }

        if(other.GetComponent<Player>() != null && other.GetComponent<Player>().hasKey()) {
            inputManager.requestSpaceInput(this, this.transform, "use key to open door");
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            inputManager.cancelSpaceInputRequest(this);
        }
    }

    public void onSpacePress() {
        if(player.hasKey()) {
            player.useKey();
            this.isOpen = true;

            this.open.SetActive(true);
            this.closed.SetActive(false);
        }
    }

    public void reset() {
        isOpen = false;
        this.open.SetActive(false);
        this.closed.SetActive(true);
    }

}
