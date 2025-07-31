using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableExtender : Effectable
{
    public List<GameObject> effectables;

    public void onEffect() {
        foreach(GameObject obj in this.effectables) {
            obj.GetComponent<Effectable>().onEffect();
        }
    }
}
