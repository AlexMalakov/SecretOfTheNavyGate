using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIO : MonoBehaviour
{
    [SerializeField] private PopUpManager manager;
    [SerializeField] private PlayerController controller;

    private InputSubscriber spaceSubscriber; //only one insteard of a list, since ive decided only 
    private InputSubscriber screenSubscriber;
    private float lastInput = 0f;
    private float lastScreenPopUp;

    public void requestSpaceInput(InputSubscriber i, Transform posOfObj, string message) {
        if(this.spaceSubscriber != i) {
            this.spaceSubscriber = i;
            this.manager.displaySpacePopUp(posOfObj, message);
        }
    }

    public void cancelInputRequest(InputSubscriber i) {
        if(this.spaceSubscriber == i) {
            this.spaceSubscriber = null;
            this.manager.endSpacePopUp();
        }
    }

    public void Update() {
        if(lastInput + .25f < Time.time && this.screenSubscriber != null && Input.GetKey(KeyCode.Space)) {
            this.manager.endScreenPopUp();
            this.controller.isMovementEnabled(true);
            this.lastScreenPopUp = Time.time;
            this.screenSubscriber = null;
        } else if(lastInput + .25f < Time.time && this.spaceSubscriber != null && Input.GetKey(KeyCode.Space)) {
            lastInput = Time.time;
            InputSubscriber s = this.spaceSubscriber;
            this.manager.endSpacePopUp();
            this.spaceSubscriber = null;
            s.onSpacePress();
        }
    }

    public void requestPopUpAlert(Transform posOfObj, string message) {
        this.manager.displayPopUpAlert(posOfObj, message);
    }

    public void requestPopUpMessage(Transform posOfObj, string message) {
        this.manager.displayPopUpMessage(posOfObj, message);
    }

    public void displayScreenPopUp(InputSubscriber i, Sprite screenImgSprite) {
        //disable player controls

        if(this.spaceSubscriber != i && this.lastScreenPopUp + .2f < Time.time) {
            this.controller.isMovementEnabled(false);
            this.spaceSubscriber = i;
            this.screenSubscriber = this.spaceSubscriber;
            this.manager.endSpacePopUp();
            this.manager.displayScreenPopUp(screenImgSprite);
        }
    }

}

public interface InputSubscriber {
    void onSpacePress();
}
