using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PopUpManager manager;

    private InputSubscriber lastSubscriber; //only one insteard of a list, since ive decided only 
    private float lastInput = 0f;

    public void requestSpaceInput(InputSubscriber i, Transform posOfObj, string message) {
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
        if(lastInput + .25f < Time.time && this.lastSubscriber != null && Input.GetKey(KeyCode.Space)) {
            lastInput = Time.time;
            InputSubscriber s = this.lastSubscriber;
            this.manager.endSpacePopUp();
            this.lastSubscriber = null;
            s.onSpacePress();
        }
    }

}

public interface InputSubscriber {
    void onSpacePress();
}
