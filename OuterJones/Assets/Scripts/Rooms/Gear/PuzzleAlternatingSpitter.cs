using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAlternatingSpitter : AlternatingSpitter, RotationPuzzleElement, InputSubscriber
{

    [SerializeField] private string enviromentLayer = "Enviroment";
    [SerializeField] private string foregroundLayer = "FrontForeground";

    [SerializeField] private GameObject cheeseBlockers;
    private Canal canal;
    private bool playerInCanal = false;

    private PlayerInput input;
    private Quaternion initialRot;

    private PlayerController controller;

    private bool startDirection;

    protected void Start() {
        this.input = FindObjectOfType<PlayerInput>();
        this.controller = FindObjectOfType<PlayerController>();

        this.initialRot = this.transform.rotation;

        this.startDirection = this.clockwise;
    }

    public void init(Canal c) {
        this.setSortingLayer(this.enviromentLayer);
        this.canal = c;
        this.playerInCanal = false;
    }


    protected override void activateAlternatingSpitter(PlayerController controller) {
        if(!this.playerInCanal)
            this.input.requestSpaceInput(this, this.transform, "rotate spinner");
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            this.input.cancelSpaceInputRequest(this);
        }
    }

    public void resetElement() {
        this.clockwise = this.startDirection;
    }

    public void onPlayerInCanal() {
        this.cheeseBlockers.SetActive(false);
        this.playerInCanal = false;
        this.setSortingLayer(this.foregroundLayer);
        this.input.cancelSpaceInputRequest(this);
        
    }

    public void onPlayerOutCanal() {
        this.cheeseBlockers.SetActive(true);
        this.playerInCanal = true;
        this.setSortingLayer(this.enviromentLayer);
    }

    public void onSpacePress() {
        if(!playerInCanal)
            StartCoroutine(rotateSpitter(this.controller));
    }

    private void setSortingLayer(string layer) {
        this.GetComponent<Renderer>().sortingLayerName = layer;

        foreach(Renderer r in this.GetComponentsInChildren<Renderer>()) {
            r.sortingLayerName = layer;
        }
    }
}
