using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Item
{
    [SerializeField] GameObject torchObj;

    public override void equip() {
        base.equip();
        torchObj.SetActive(true);
        torchObj.GetComponent<TorchCollider>().setActiveStatus(true);
    }

    public override void unequip() {
        base.unequip();
        torchObj.SetActive(false);
        torchObj.GetComponent<TorchCollider>().setActiveStatus(false);
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Torch;
    }

    public override bool startsEquiped() {return true;}
    public override bool canBeToggled() {return true;}
}
