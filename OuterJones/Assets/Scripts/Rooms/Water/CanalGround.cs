using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalGround : MonoBehaviour
{
    [SerializeField] private Canal canal;

    public Canal getCanal() {
        return this.canal;
    }
}
