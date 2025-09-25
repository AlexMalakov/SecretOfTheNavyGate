using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//stuff i want to dynamically track:
//light receivers, light sources
//r1 water wheel
//corner pieces
//chests, keys,

public class ObjListener : MonoBehaviour
{
    [SerializeField] protected ListenerController controller;

    [SerializeField] protected Image img;
    [SerializeField] protected Sprite activatedSprite;
    [SerializeField] protected Sprite unactivatedSprite;

    [SerializeField] private bool requestsExclamation;

    protected bool isActive;

    public void Awake() {
        img.sprite = unactivatedSprite;
        img.type = Image.Type.Simple;
        img.preserveAspect = false;
    }

    public void onStatusChanged(bool isActive) {
        this.isActive = isActive;
        img.sprite = this.isActive ? this.activatedSprite : this.unactivatedSprite;

        if(this.isActive && this.requestsExclamation) {
            this.controller.requestRoomExclamation();
        }
    }
}
