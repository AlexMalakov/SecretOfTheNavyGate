using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject torchObj;

    public void equip() {
        torchObj.SetActive(true);
        torchObj.GetComponent<TorchCollider>().setActiveStatus(true);
    }

    public void unequip() {
        torchObj.SetActive(false);
        torchObj.GetComponent<TorchCollider>().setActiveStatus(false);
    }
}
