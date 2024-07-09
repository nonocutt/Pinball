using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    private AudioSource _source;
    public enum StateMachine
    {
        Play, // 0
        Pause // 1
    }

    public StateMachine State;

    [SerializeField] private TextMeshProUGUI _pauseGUI;
    [SerializeField] private TextMeshProUGUI _gameoverGUI;
    [SerializeField] private TextMeshProUGUI _livesGUI;
    [SerializeField] private TextMeshProUGUI _scoreGUI;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int maxLives = 9;
    [SerializeField] private int bonus = 5000;
    [SerializeField] private float lifeLossProtectionDuration = 10.0f;
    [SerializeField] private AudioClip _loselife;
    [SerializeField] private AudioClip _gameover;
    [SerializeField] private AudioClip _pause;
    [SerializeField] private AudioClip _newlife;

    private int currentLives;
    private int score = 0;
    private int nextLifeThreshold;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();
        UpdateScoreText();
    }

    void ResetGame()
    {
        score = 0; // Reset score for player
        nextLifeThreshold = bonus; // Reset the next life threshold
        State = StateMachine.Play;
        _pauseGUI.enabled = false;
        _gameoverGUI.enabled = false;
        currentLives = startingLives;
        UpdateLifeUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScreen");
        }

        if (currentLives <= 0 && State != StateMachine.Pause)
        {
            GameOver();
        }

        if (State == StateMachine.Pause && _gameoverGUI.enabled && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
    }

    public void LoseLife()
    {
        PlaySound(_loselife);
        currentLives--;
        UpdateLifeUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void TogglePause()
    {
        State = State == StateMachine.Play ? StateMachine.Pause : StateMachine.Play;
        PlaySound(_pause);
        _pauseGUI.enabled = !_pauseGUI.enabled; // flipping the boolean
        Time.timeScale = State == StateMachine.Pause ? 0 : 1; // Pause or resume time
    }

    private void GameOver()
    {
        PlaySound(_gameover);
        State = StateMachine.Pause;
        _gameoverGUI.enabled = true;
        _pauseGUI.enabled = false;
    }

    private void UpdateLifeUI()
    {
        _livesGUI.text = "LIFE: " + currentLives;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        // Check if score reached the next threshold for an extra life
        while (score >= nextLifeThreshold)
        {
            if (currentLives < maxLives)
            {
                currentLives++;
                UpdateLifeUI();
            }
            PlaySound(_newlife);
            nextLifeThreshold += bonus; // Increment the threshold for the next life
        }
    }

    // Method to update the TextMeshPro Text with the current score
    private void UpdateScoreText()
    {
        _scoreGUI.text = "PLAYER\n" + score;
    }
    private void PlaySound(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
    }
}