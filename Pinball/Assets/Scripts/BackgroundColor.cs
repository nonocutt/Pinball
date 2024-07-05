using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.backgroundColor = GetRandomColor();
    }
    
    
    Color GetRandomColor()
    {
        float r = Random.Range(0, 128) / 255f;
        float g = Random.Range(0, 128) / 255f;
        float b = Random.Range(0, 128) / 255f;
        return new Color(r, g, b);
    }
}
