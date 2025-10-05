using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableExtender : MonoBehaviour, Effectable
{
    public List<GameObject> effectables;

    public void onEffect() {
        foreach(GameObject obj in this.effectables) {
            obj.GetComponent<Effectable>().onEffect();
        }
    }

    public void onEffectOver() {
        foreach(GameObject obj in this.effectables) {
            obj.GetComponent<Effectable>().onEffectOver();
        }
    }
}
