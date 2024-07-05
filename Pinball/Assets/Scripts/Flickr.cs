using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Flickr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gui;

    [Range(0.1f, 1.0f)]
    [SerializeField] private float _flickerRate = 0.5f;
    
    private void Start()
    {
        StartCoroutine(flicker());
    }
    
    IEnumerator flicker()
    {
        while (true)
        {
            _gui.enabled = !_gui.enabled;
            yield return new WaitForSeconds(_flickerRate);
        }
    }
}
