using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderbellyStaircase : MonoBehaviour, Effectable, InputSubscriber
{
    [SerializeField] private UnderbellyStaircase destination;
    [SerializeField] private Player player;

    [SerializeField] private Transform exitPos;

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
            FindObjectOfType<Map>().onUnderbellyUnlock(this.originRoom);
            this.destination.onEffect();
            //ping map to show room change
        }
    }

    public void onEffectOver(){}//doors stay open cuz otherwise it would be annoying lol

    public Transform getEnterPos() {
        return this.exitPos;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(this.opened && other.GetComponent<Player>() != null) {
            this.input.requestSpaceInput(this, this.exitPos, (this.originRoom.getPosition().overworld ? "descend" : "ascend"));
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(this.opened && other.GetComponent<Player>() != null) {
            this.input.cancelInputRequest(this);
        }
    }

    public void onSpacePress() {
        this.originRoom.onExit();
        this.destination.onEntered();
    }

    public void onEntered() {
        this.originRoom.onEnter(this);
        this.player.setCurrentRoom(this.originRoom);
        this.player.transform.position = this.exitPos.position;

        //door use listenering :)
    }
}
