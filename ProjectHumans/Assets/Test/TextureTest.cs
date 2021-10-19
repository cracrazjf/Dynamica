using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureTest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

        var tex = new Texture2D(2, 2, TextureFormat.RGBAFloat, true);
        var data = new float[]
        {
            // mip 0: 2x2 size
            254.5f, 0, 0, 1, // red
            0, 255, 0, 1, // green
            0, 0, 255, 1, // blue
            255, 235, 4, 1// yellow
        };
        tex.SetPixelData(data, 0, 0); // mip 0

        tex.filterMode = FilterMode.Point;
        tex.Apply(updateMipmaps: false);

        GetComponent<RawImage>().texture = tex;

    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
