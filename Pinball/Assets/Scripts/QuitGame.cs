using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// pre-processor
#if UNITY_EDITOR
    using UnityEditor;
#endif
public class QuitGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Quit();
        }
    }

    void Quit()
    {
        #if UNITY_EDITOR
            // will be called if running through unity
            EditorApplication.isPlaying = false;
        #else
            // if running from a build
            Application.Quit();
        #endif
    }
}
