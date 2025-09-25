using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapImageWrangler : MonoBehaviour
{

    Image[] images;

    public void init() {
        this.images = GetComponentsInChildren<Image>(includeInactive: true);
    }

    public Image getImageAt(int x, int y) {
        return this.images[5*y+x];
    }
}
