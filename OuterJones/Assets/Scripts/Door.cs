using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Door : MonoBehaviour 
{
    [SerializeField] private Door destination;
    [SerializeField] private Room room;
    [SerializeField] private Transform enterPosition;

    private float disabledDuration = 3f;
    private bool canUse = true;

    
    public void changeDestination(Door newDestination) {
        this.destination = newDestination;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(!canUse) {
            return;
        }

        if(other.gameObject.GetComponent<PlayerController>() != null) {
            this.onExit();
            this.destination.onEnter(other.gameObject.GetComponent<PlayerController>());
        }
    }

    private void onExit() {
        this.room.onExit();
    }

    public void onEnter(PlayerController player) {
        player.transform.position = enterPosition.position;
        this.canUse = false;
        Invoke(nameof(enableDoor), this.disabledDuration);
        this.room.onEnter();
    }

    private void enableDoor() {
        this.canUse = true;
    }
}