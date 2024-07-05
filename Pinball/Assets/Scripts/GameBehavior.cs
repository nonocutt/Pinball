using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    // `static` ensures that there is only of something

    public enum StateMachine
    {
        Play,   // 0
        Pause   // 1
    }

    public StateMachine State;


    [SerializeField] private TextMeshProUGUI _pauseGUI;
    
    public Player[] Players = new Player[2];

    [Header("1UP")]
    [SerializeField] int ScoreGoal = 10000;
    
    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)   // if exist
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetGame();
    }

    void ResetGame()
    {
        foreach (Player p in Players)       // temporary placeholder
        {
            p.Score = 0;
        }

        State = StateMachine.Play;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            State = State == StateMachine.Play ? StateMachine.Pause : StateMachine.Play;
            _pauseGUI.enabled = !_pauseGUI.enabled;     // flipping the boolean
        }
        
        if (Input.GetKeyUp(KeyCode.Escape)) {
                SceneManager.LoadScene("TitleScreen");
        }
        
    }
}
