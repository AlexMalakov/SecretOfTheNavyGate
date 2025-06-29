using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireDoor : MonoBehaviour, PowerableObject
{
    [SerializeField] private bool startsOpen;
    [SerializeField] private GameObject openSprite;
    [SerializeField] private GameObject closedSprite;

    [SerializeField] private PowerableObject nextToPower;


    public void onPowered() {
        this.openSprite.SetActive(startsOpen);
        this.closedSprite.SetActive(!startsOpen);

        nextToPower.onPowered();
    }

    public void reset() {
        this.openSprite.SetActive(startsOpen);
        this.closedSprite.SetActive(!startsOpen);
    }
    
}
