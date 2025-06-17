using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanRoom : Room
{
    [SerializeField] private Mummy mummy;
    private Button[] buttons;

    public void Start() {
        this.buttons = GetComponentsInChildren<Button>();
    }

    public override void onEnter() {
        base.onEnter();
        mummy.wake();
    }

    public override void onExit() {
        mummy.sleep();
        base.onExit();
    }


    public static bool isPackmanPlace(Door origin, int maxX, int maxY) {

        return (origin.getDirection() == DoorDirection.North  && origin.getPosition().getOffset(0, 1).y == maxY)
                || (origin.getDirection() == DoorDirection.East && origin.getPosition().getOffset(1, 0).x == maxX)
                || (origin.getDirection() == DoorDirection.West && origin.getPosition().getOffset(-1, 0).x == 0)
                || (origin.getDirection() == DoorDirection.South && origin.getPosition().getOffset(0, -1).y == 0);
    }

    public void onButtonEvent(Button b, bool isPressed) {

    }
}
