using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraInit : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Utilities.SetHeightsInUnits(cam);
    }
}
