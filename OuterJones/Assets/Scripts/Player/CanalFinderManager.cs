using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalFinderManager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    private List<PlayerCanalFinders> canalFinders = new List<PlayerCanalFinders>();
    private bool inCanal = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PlayerCanalFinders cf in this.GetComponentsInChildren<PlayerCanalFinders>()) {
            canalFinders.Add(cf);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Canal>() != null) {
            this.inCanal = false;
        }
    }

    public Transform fallInCanal(Canal c) {

        List<PlayerCanalFinders> options = new List<PlayerCanalFinders>();

        foreach(PlayerCanalFinders cf in this.canalFinders) {
            if(cf.collidingWithCanal(c)) {
                options.Add(cf);
            }
        }

        if(options.Count == 0) {
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

        return options[bestInd].transform;
    }

    public IEnumerator shoveIntoCanal(Transform destination) {
        if(this.inCanal) {
            yield break;
        }
        Debug.Log("MOVING!");
        this.inCanal = true;
        this.controller.isMovementEnabled(false);

        Vector3 initial = this.controller.transform.position;
        Vector3 final = destination.position;

        float elapsed = 0f;

        while(elapsed < .25f) {
            this.controller.transform.position = Vector3.Lerp(initial, final, elapsed/.25f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        this.controller.isMovementEnabled(true);
    }

    public bool isInCanal() {
        return this.inCanal;
    }
}
