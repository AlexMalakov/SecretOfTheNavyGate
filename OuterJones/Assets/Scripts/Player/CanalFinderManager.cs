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

    public void OnTriggerEnter2D(Collider2D other) {
        List<PlayerCanalFinders> options = new List<PlayerCanalFinders>();
        if(other.GetComponent<Canal>() != null) {
            Canal c = other.GetComponent<Canal>();

            foreach(PlayerCanalFinders cf in this.canalFinders) {
                if(cf.collidingWithCanal(other.GetComponent<Canal>())) {
                    options.Add(cf);
                }
            }
        }

        if(options.Count == 0) {
            Debug.Log("ERROR CANT DETECT CANAL COLLISION!");
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
        this.controller.isMovementEnabled(false);

        Vector3 initial = this.controller.transform.position;

        float elapsed = 0f;

        while(elapsed < 1f) {
            this.controller.transform.position = Vector3.Lerp(initial, destination.position, elapsed);

            elapsed += Time.deltaTime;
            yield return null;
        }

        this.controller.isMovementEnabled(true);
    }
}
