using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplePoint : MonoBehaviour, InputSubscriber
{
    [SerializeField] GrappleZone z1;
    [SerializeField] GrappleZone z2;

    [SerializeField] Transform z1Landing;
    [SerializeField] Transform z2Landing;

    private PlayerIO input;
    private PlayerController controller;

    private bool playerHasWhip = false;

    int lastID = 0;

    public void setWhipStatus(bool status) {
        this.playerHasWhip = status;
    }
    
    void Awake() {
        z1.init(this, 1);
        z2.init(this, 2);

        this.input = FindObjectOfType<PlayerIO>();
        this.controller = FindObjectOfType<PlayerController>();
    }

    public void grapleFrom(int id) {
        if(this.playerHasWhip) {
            this.lastID = id;
            this.input.requestSpaceInput(this, transform, "swing");
        }
    }

    public void pointExit() {
        this.input.cancelRequest(this);
    }


    public void onSpacePress() {
        if(lastID == 1) {
            StartCoroutine(swingPlayer(z2Landing.position));
        } else if(lastID == 2) {
            StartCoroutine(swingPlayer(z1Landing.position));
        }
    }

    public IEnumerator swingPlayer(Vector3 target) {
        this.controller.isMovementEnabled(false);

        Vector3 startPos = this.controller.transform.position;

        float elapsed = 0f;

        while(elapsed < .5f) {
            this.controller.transform.position = Vector3.Lerp(startPos, target, elapsed/.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.controller.isMovementEnabled(true);
    }
}
