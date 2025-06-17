using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapImageWrangler : MonoBehaviour
{

    Image[] images;

    void Awake()
    {
        this.images = GetComponentsInChildren<Image>(includeInactive: true);

        // int counter = 0;
        // foreach(Image i in this.images) {
        //     Debug.Log(counter + " = "+ i.gameObject.name);
        //     counter++;
        // }
    }

    public Image getImageAt(int x, int y) {
        return this.images[5*y+x];
    }
}
