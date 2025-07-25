using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableDoor : MonoBehaviour, Effectable
{
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject horizontalClosed;
    [SerializeField] private GameObject verticalClosed;
    [SerializeField] private bool vert;

    [SerializeField] private bool initiallyOpen;
    private bool openState;

    public void Awake() {
        this.openState  = initiallyOpen;

        open.SetActive(openState);
        horizontalClosed.SetActive(!openState && !vert);
        verticalClosed.SetActive(!openState && vert);
    }

    public void onEffect() {
        openState = !openState;
        open.SetActive(openState);
        horizontalClosed.SetActive(!openState && !vert);
        verticalClosed.SetActive(!openState && vert);
    }

    //if specifically open/close is wanted
    public void opencloseDoor(bool openDesired) {
        openState = openDesired;
        open.SetActive(openState);
        horizontalClosed.SetActive(!openState && !vert);
        verticalClosed.SetActive(!openState && vert);
    }

    public void rotate90() {
        vert = !vert;
        
        open.SetActive(openState);
        horizontalClosed.SetActive(!openState && !vert);
        verticalClosed.SetActive(!openState && vert);
    }
}
