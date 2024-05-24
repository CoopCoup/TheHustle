using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    public int width, height;
    private bool fullscreen = true;

    void Awake()
    {
        width = Screen.currentResolution.width;
        height = Screen.currentResolution.height;
    }

    void Update()
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(width, height, true);
        }
        else Screen.fullScreenMode = FullScreenMode.Windowed;

        if (Input.GetKeyDown(KeyCode.F11)) fullscreen = !fullscreen;
    }
}
