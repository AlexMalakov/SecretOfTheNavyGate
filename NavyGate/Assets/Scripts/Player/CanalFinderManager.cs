using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalFinderManager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    private List<PlayerCanalFinders> canalFinders = new List<PlayerCanalFinders>();
    private Canal canalImIn = null;
    private bool fallingIntoCanal = false;
    // private bool fallingIntoCanal = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PlayerCanalFinders cf in this.GetComponentsInChildren<PlayerCanalFinders>()) {
            canalFinders.Add(cf);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Canal>() != null && other.GetComponent<Canal>() == this.canalImIn) {
            this.canalImIn = null;
            Debug.Log("LEFT CANAL " + other.gameObject.name);
        }
    }

    public Transform fallInCanal(Canal c) {
        // fallingIntoCanal = true;
        // Debug.Log("falling into " + c.gameObject.name);
        List<PlayerCanalFinders> options = new List<PlayerCanalFinders>();

        foreach(PlayerCanalFinders cf in this.canalFinders) {
            if(cf.collidingWithCanal(c)) {
                options.Add(cf);
            }
        }

        if(options.Count == 0) {
            Debug.Log("using backup!");
            return c.getClosestBackup(this.transform);
        }

        int bestInd = 0;

        Vector3 target = this.controller.getDirection3D() + this.transform.position;
        float best = (target - options[0].transform.position).magnitude;


        for(int i = 1; i < options.Count; i++) {
            if((target - options[i].transform.position).magnitude < best) {
                bestInd = i;
                best = (target - options[i].transform.position).magnitude;
            }
        }

        // Debug.Log("CHOSEN FINDER: " + options[bestInd].gameObject.name);
        return options[bestInd].transform;
    }

    public IEnumerator shoveIntoCanal(Transform destination, Canal canalImGoingIn) {
        if(this.canalImIn != null) {
            yield break;
        }
        
        fallingIntoCanal = true;
        // Debug.Log("MOVING INTO " + canalImGoingIn.gameObject.name);
        this.canalImIn = canalImGoingIn;
        this.controller.isMovementEnabled(false);

        Vector3 initial = this.controller.transform.position;
        Vector3 final = destination.position;

        float elapsed = 0f;

        while(elapsed < .25f) {
            this.controller.transform.position = Vector3.Lerp(initial, final, elapsed/.25f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        fallingIntoCanal = false;
        this.controller.isMovementEnabled(true);
    }

    private IEnumerator fallOutOfBorder(Canal canalImGoingIn) {
        if(this.canalImIn == null || this.fallingIntoCanal) {
            yield break;
        }

        this.fallingIntoCanal = true;
        this.controller.isMovementEnabled(false);
        Vector3 initial = this.controller.transform.position;
        Vector3 final = fallInCanal(canalImGoingIn).position;

        float elapsed = 0f;

        while(elapsed < .25f) {
            this.controller.transform.position = Vector3.Lerp(initial, final, elapsed/.25f);

            elapsed += Time.deltaTime;
            yield return null;
        }


        this.fallingIntoCanal = false;
        this.controller.isMovementEnabled(true);
    }

    public bool isInCanal() {
        return this.canalImIn != null;
    }
}
