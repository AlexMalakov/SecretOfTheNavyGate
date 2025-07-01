using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grate : MonoBehaviour
{
    [SerializeField] string enviromentLayer = "Enviroment";
    [SerializeField] string foregroundLayer = "FrontForeground";

    private Renderer rend;


    public void Start() {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("No Renderer component found!");
        }
        else
        {
            rend.sortingLayerName = this.enviromentLayer;
        }
    }

    public void onPlayerInCanal() {
        rend.sortingLayerName = this.foregroundLayer;
    }

    public void onPlayerOutCanal() {
        rend.sortingLayerName = this.enviromentLayer;
    }
}
