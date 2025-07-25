using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotatingGear : RotatingGear, RotationPuzzleElement, InputSubscriber
{
    [SerializeField] private string enviromentLayer = "Enviroment";
    [SerializeField] private string foregroundLayer = "FrontForeground";

    [SerializeField] private GameObject cheeseBlockers;
    private Canal canal;
    private bool playerInCanal = false;

    private PlayerInput input;
    private Quaternion initialRot;
    private PlayerController controller;
    private GearTooth recent;

    [SerializeField] bool oneWay;

    protected override void Start() {
        base.Start();
        this.input = FindObjectOfType<PlayerInput>();

        this.initialRot = this.transform.rotation;
    }

    public void init(Canal c) {
        this.setSortingLayer(this.enviromentLayer);
        this.canal = c;
        this.playerInCanal = false;
    }

    public override void playerOnTooth(GearTooth t, PlayerController controller) {
        if(playerInCanal)
            return;
        this.controller = controller;
        this.recent = t;
        this.input.requestSpaceInput(this, this.transform, "rotate platform");
    }

    public override void playerOffTooth() {
        this.input.cancelSpaceInputRequest(this);
    }

    public void onSpacePress() {
        if(playerInCanal)
            return;

        if(oneWay) {
            base.playerOnTooth(this.recent, this.controller);
        } else {
            StartCoroutine(this.rotateGear(this.controller));
        }

    }

    public void resetElement() {
        this.transform.rotation = this.initialRot;
    }

    public void onPlayerInCanal() {
        this.playerInCanal = false;
        this.cheeseBlockers.SetActive(false);
        this.setSortingLayer(this.foregroundLayer);
        this.input.cancelSpaceInputRequest(this);
        
    }

    public void onPlayerOutCanal() {
        this.playerInCanal = true;
        this.cheeseBlockers.SetActive(true);
        this.setSortingLayer(this.enviromentLayer);
    }

    private void setSortingLayer(string layer) {
        this.GetComponent<Renderer>().sortingLayerName = layer;

        foreach(Renderer r in this.GetComponentsInChildren<Renderer>()) {
            r.sortingLayerName = layer;
        }
    }
}
