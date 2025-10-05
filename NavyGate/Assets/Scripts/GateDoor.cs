using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDoor : MonoBehaviour
{
    [SerializeField] protected GameObject vertOpen;
    [SerializeField] protected GameObject horizOpen;
    [SerializeField] protected GameObject vertClosed;
    [SerializeField] protected GameObject horizClosed;

    [SerializeField] protected bool vert;

    protected bool openState;
    private Quaternion initialRot;


    protected virtual void Awake() {
        this.initialRot = transform.rotation;
        flipSprites();
    }

    public void toggleOpen() {
        openState = !openState;
        flipSprites();
    }

    public void rotate90(bool clockwise) {
        vert = !vert;
        
        this.transform.rotation = this.initialRot;
        flipSprites();
    }

    protected void flipSprites() {
        horizOpen.SetActive(openState && !vert);
        vertOpen.SetActive(openState && vert);
        horizClosed.SetActive(!openState && !vert);
        vertClosed.SetActive(!openState && vert);
    }

}
