using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDoor : MonoBehaviour
{
    [SerializeField] protected GameObject open;
    [SerializeField] protected GameObject vertClosed;
    [SerializeField] protected GameObject horizClosed;

    [SerializeField] protected bool vert;

    protected bool openState;
    private Quaternion initialRot;


    protected virtual void Awake() {
        this.initialRot = transform.rotation;
        open.SetActive(openState);
        horizClosed.SetActive(!openState && !vert);
        vertClosed.SetActive(!openState && vert);
    }

    public void toggleOpen() {
        openState = !openState;
        open.SetActive(openState);
        horizClosed.SetActive(!openState && !vert);
        vertClosed.SetActive(!openState && vert);
    }

    public void rotate90(bool clockwise) {
        vert = !vert;
        
        this.transform.rotation = this.initialRot;
        open.SetActive(openState);
        horizClosed.SetActive(!openState && !vert);
        vertClosed.SetActive(!openState && vert);
    }

}
