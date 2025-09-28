using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, InputSubscriber
{

    [SerializeField] private PlayerIO io;
    [SerializeField] private Sprite noteInfo;

    
    
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            io.requestSpaceInput(this, this.transform, "read note");
        }
    }


    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            io.cancelInputRequest(this);
        }
    }

    public void onSpacePress() {
        io.displayScreenPopUp(this, this.noteInfo);
    }
}
