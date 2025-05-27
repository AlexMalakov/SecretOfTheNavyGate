using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [SerializeField] private float roomLighting = .5f;
    [SerializeField] private Light2D globalLighting;
    [SerializeField] private GameObject cameraObj;

    public void onEnter() {
        this.gameObject.SetActive(true);
        cameraObj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y ,this.cameraObj.transform.position.z);
        globalLighting.intensity = this.roomLighting;
    }

    public void onExit() {
        this.gameObject.SetActive(false);
    }
}