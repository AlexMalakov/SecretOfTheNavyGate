using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldNotifier : MonoBehaviour, Effectable
{
    [SerializeField] private ForcefieldManager manager;
    [SerializeField] private RoomType deactivateType;

    public void onEffect() {
        manager.deactivateForceField(this.deactivateType);
    }
}
