using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableDoor : MonoBehaviour, Effectable
{
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject closed;

    [SerializeField] private bool initiallyOpen;
    private bool openState;

    public void Awake() {
        this.openState  = initiallyOpen;

        open.SetActive(openState);
        closed.SetActive(!openState);
    }

    public void onEffect() {
        openState = !openState;
        open.SetActive(openState);
        closed.SetActive(!openState);
    }
}
