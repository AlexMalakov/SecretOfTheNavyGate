using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] GameObject open;
    [SerializeField] GameObject closed;

    private bool isOpen = false;

    public void OnTriggerEnter2D(Collider2D other) {
        if(isOpen) {
            return;
        }

        if(other.GetComponent<Player>() != null) {
            //display space bar pop up message
        }

        if(other.GetComponent<Player>() != null && PlayerInput.getSpaceInput(this.transform)) {
            //attempt to open door
            if(other.GetComponent<Player>().hasKey()) {
                other.GetComponent<Player>().useKey();
                this.isOpen = true;

                this.open.SetActive(true);
                this.closed.SetActive(false);
            }
        }
    }

}
