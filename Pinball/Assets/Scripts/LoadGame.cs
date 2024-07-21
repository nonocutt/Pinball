using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{

    [SerializeField] private GameObject _helpGUI;
    
    void Update()
    {
        if (!_helpGUI.activeSelf && Input.GetKeyUp(KeyCode.Return))
        {
            SceneManager.LoadScene("Pinball");
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            ToggleHelp();
        }
    }
    
    private void ToggleHelp()
    {
        _helpGUI.SetActive(!_helpGUI.activeSelf);
    }
}
