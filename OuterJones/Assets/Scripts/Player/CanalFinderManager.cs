using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalFinderManager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    private List<PlayerCanalFinders> canalFinders = new List<PlayerCanalFinders>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(PlayerCanalFinders cf in this.GetComponentsInChildren<PlayerCanalFinders>()) {
            canalFinders.Add(cf);
        }
    }

    public void fallInCanal(Canal c) {

        List<PlayerCanalFinders> options = new List<PlayerCanalFinders>();

        foreach(PlayerCanalFinders cf in this.canalFinders) {
            if(cf.collidingWithCanal(c)) {
                options.Add(cf);
            }
        }

        if(options.Count == 0) {
            StartCoroutine(shoveIntoCanal(c.getClosestBackup(this.transform)));
            return;
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

        StartCoroutine(shoveIntoCanal(options[bestInd].transform));
    }

    private IEnumerator shoveIntoCanal(Transform destination) {
        Debug.Log("MOVING!");
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
}
