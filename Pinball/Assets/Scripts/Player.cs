using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _score;

    public int Score
    {
        /*
        get
        
        {
            return _score;
        }
        */
        
        get => _score;
        set
        {
            _score = value;     // value is what being passed on
            ScoreGui.text = Score.ToString();
        }
    }

    [SerializeField] private TextMeshProUGUI ScoreGui;
    

}
