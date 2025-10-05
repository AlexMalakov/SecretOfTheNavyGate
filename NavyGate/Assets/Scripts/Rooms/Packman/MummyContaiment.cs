using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyContaiment : MonoBehaviour, Effectable
{

    [SerializeField] private MummyManager manager;

    public void onEffect() {
        this.manager.swapTargets(true);
    }

    public void onEffectOver() {
        
    }
}
