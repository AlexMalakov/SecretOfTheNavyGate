using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public enum PossibleItems {
    Amulet, Floaties, GearItem, Torch, Whip, Key
}
public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    [SerializeField] private List<Image> hotbarImages;
    [SerializeField] private List<Image> hotbarImageBackgrounds;
    [SerializeField] private Key startingKey;
    [SerializeField] private TMP_Text key_count;

    private Dictionary<PossibleItems, List<ItemListener>> itemListeners = new Dictionary<PossibleItems, List<ItemListener>>();

    void Awake() {
        this.gainItem(startingKey);
    }

    public void addItemListener(PossibleItems itemType, ItemListener l) {
        if(this.itemListeners.ContainsKey(itemType)) {
            this.itemListeners[itemType].Add(l);
        } else {
            this.itemListeners.Add(itemType, new List<ItemListener>(){l});
        }
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1) && items.Count >= 2) {
            equipItemN(1);
        } else if(Input.GetKeyDown(KeyCode.Alpha2) && items.Count >= 3) {
            equipItemN(2);
        } else if(Input.GetKeyDown(KeyCode.Alpha3) && items.Count >= 4) {
            equipItemN(3);
        } else if(Input.GetKeyDown(KeyCode.Alpha4) && items.Count >= 5) {
            equipItemN(4);
        } else if(Input.GetKeyDown(KeyCode.Alpha5) && items.Count >= 6) {
            equipItemN(5);
        }
    }

    public void onKeyUpdate() {
        key_count.text = ""+((Key)this.items[0]).getKeyCount();
        this.setColorOfItemBg(0);
    }

    public void gainItem(Item newItem) {
        if(newItem is Key) {
            foreach(Item i in this.items) {
                if(i is Key) {
                    ((Key) i).gainKey();
                    return;
                }
            }
        }

        this.items.Add(newItem);
        this.hotbarImages[this.items.Count - 1].sprite = newItem.getItemIcon();
        if(newItem.startsEquiped()) {
            newItem.equip();

            this.notifyOfEvent(true, newItem);
        }
        setColorOfItemBg(this.items.Count - 1);
    }

    private void equipItemN(int n) {
        if(this.items[n].canBeToggled()) {
            if(this.items[n].isEquiped()) {
                this.items[n].unequip();
                this.notifyOfEvent(false, this.items[n]);
                
            } else {
                this.items[n].equip();
                this.notifyOfEvent(true, this.items[n]);
            }
            this.setColorOfItemBg(n);
        }
    }

    private void notifyOfEvent(bool equiped, Item i) {
        if(this.itemListeners.ContainsKey(i.getItemType())) {
            foreach(ItemListener l in this.itemListeners[i.getItemType()]) {
                l.onItemEvent(equiped);
            }
        }
    }

    private void setColorOfItemBg(int n) {
        this.hotbarImageBackgrounds[n].color = (this.items[n].isEquiped()) ? new Color32(255, 177, 90, 255) : new Color32(0, 0, 0, 100);
    }

    public bool hasItem(PossibleItems t) {
        foreach(Item i in this.items) {
            if(i.getItemType() == t) {
                return true;
            }
        }
        return false;
    }

    public Item getItem(PossibleItems t) {
        foreach(Item i in this.items) {
            if(i.getItemType() == t) {
                return i;
            }
        }
        return null;
    }

}
