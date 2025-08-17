using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RotationPuzzleElement : MonoBehaviour
{
    [SerializeField] private string enviromentLayer = "Enviroment";
    [SerializeField] private string foregroundLayer = "FrontForeground";

    [SerializeField] private GameObject colliderObj;

    protected bool playerInCanal = false;
    private Quaternion initialRot;
    
    void Awake() {
        this.initialRot = this.transform.localRotation;
    }

    public virtual void resetElement() {
        this.transform.localRotation = this.initialRot;
    }

    public void init() {
        this.setSortingLayer(this.enviromentLayer);
        this.playerInCanal = false;
    }

    public virtual void onPlayerInCanal() {
        this.playerInCanal = true;
        this.colliderObj.SetActive(false);
        this.setSortingLayer(this.foregroundLayer);
    }

    public void onPlayerOutCanal() {
        this.playerInCanal = false;
        this.colliderObj.SetActive(true);
        this.setSortingLayer(this.enviromentLayer);
    }

    private void setSortingLayer(string layer) {

        foreach(Renderer r in this.GetComponentsInChildren<Renderer>(true)) {
            r.sortingLayerName = layer;
        }
    }
} 

