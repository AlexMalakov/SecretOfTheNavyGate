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
    private bool colliding = false;

    int lastID = 0;

    public void setWhipStatus(bool status) {
        this.playerHasWhip = status;
        if(status && this.colliding) {
            this.input.requestSpaceInput(this, transform, "swing");
        }
    }
    
    void Awake() {
        z1.init(this, 1);
        z2.init(this, 2);

        this.input = FindObjectOfType<PlayerIO>();
        this.controller = FindObjectOfType<PlayerController>();
    }

    public void grapleFrom(int id) {
        this.lastID = id;
        this.colliding = true;
        if(this.playerHasWhip && this.controller.gameObject.GetComponent<Player>().canGrapple()) {
            this.input.requestSpaceInput(this, transform, "swing");
        }
    }

    public void pointExit() {
        this.colliding = false;
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
        Player p = this.controller.gameObject.GetComponent<Player>();
        p.setGrapplingState(true);
        this.controller.isMovementEnabled(false);

        Vector3 startPos = this.controller.transform.position;

        float elapsed = 0f;

        while(elapsed < .5f) {
            this.controller.transform.position = Vector3.Lerp(startPos, target, elapsed/.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }


        yield return new WaitForFixedUpdate();
        this.controller.isMovementEnabled(true);
        p.setGrapplingState(false);
    }
}
