using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PopUpManager manager;

    private InputSubscriber lastSubscriber; //only one insteard of a list, since ive decided only 

    public void requestSpaceInput(InputSubscriber i, Transform posOfObj, string message) {
        this.manager.displaySpacePopUp(posOfObj, message);
        if(this.lastSubscriber != i) {
            this.lastSubscriber = i;
            this.manager.displaySpacePopUp(posOfObj, message);
        }
    }

    public void cancelSpaceInputRequest(InputSubscriber i) {
        if(this.lastSubscriber == i) {
            this.lastSubscriber = null;
            this.manager.endSpacePopUp();
        }

    }

    public void Update() {
        if(this.lastSubscriber != null && Input.GetKey(KeyCode.Space)) {
            this.lastSubscriber.onSpacePress();
            this.lastSubscriber = null;
            this.manager.endSpacePopUp();
        }
    }
}

public interface InputSubscriber {
    void onSpacePress();
}
