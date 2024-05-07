using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float targetWidth = 320f;
    public float targetHeight = 240f;
    
    // Start is called before the first frame update
    void Start()
    {
        float targetAspect = (float)targetWidth / (float)targetHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            camera.orthographicSize *= scaleHeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
