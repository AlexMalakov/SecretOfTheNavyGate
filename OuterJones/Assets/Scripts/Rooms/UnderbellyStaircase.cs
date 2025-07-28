using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderbellyStaircase : MonoBehaviour, Effectable, InputSubscriber
{
    [SerializeField] private GameObject lid;
    [SerializeField] private GameObject stairs;

    [SerializeField] private Room originRoom;

    private bool opened = false;
    private PlayerIO input;


    public void Awake() {
        this.input = FindObjectOfType<PlayerIO>();
    }

    public void onEffect() {
        if(!this.opened) {
            this.opened = true;
            this.lid.SetActive(false);
            this.stairs.SetActive(true);

            //ping map to show room change
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(this.opened && other.GetComponent<Player>() != null) {
            this.input.requestSpaceInput(this, this.transform, "descend");
        }
    }

    public void onSpacePress() {
        Debug.Log("ENTERING THE UNDERBELLY!");
        //enter the underbelly
    }
}
