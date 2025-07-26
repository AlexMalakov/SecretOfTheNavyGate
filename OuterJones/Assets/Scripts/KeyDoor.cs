using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour, InputSubscriber
{
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject vertClosed;
    [SerializeField] private GameObject horizClosed;

    [SerializeField] private bool vert;

    private PlayerInput inputManager;
    private Player player;

    private bool isOpen = false;

    public void Awake() {
        this.inputManager = FindObjectOfType<PlayerInput>();
        this.player = FindObjectOfType<Player>();

        vertClosed.SetActive(vert);
        horizClosed.SetActive(!vert);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(isOpen) {
            return;
        }

        if(other.GetComponent<Player>() != null && other.GetComponent<Player>().hasKey()) {
            inputManager.requestSpaceInput(this, this.transform, "open door with key");
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
            this.vertClosed.SetActive(false);
            this.horizClosed.SetActive(false);
        }
    }

    public void rotate90() {
        vert = !vert;

        vertClosed.SetActive(vert);
        horizClosed.SetActive(!vert);
    }
}