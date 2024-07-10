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

    public StateMachine State { get; private set; }

    [SerializeField] private TextMeshProUGUI _pauseGUI;
    [SerializeField] private TextMeshProUGUI _gameoverGUI;
    [SerializeField] private TextMeshProUGUI _livesGUI;
    [SerializeField] private TextMeshProUGUI _scoreGUI;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int maxLives = 9;
    [SerializeField] private int bonus = 5000;
    [SerializeField] private AudioClip _loselife;
    [SerializeField] private AudioClip _gameover;
    [SerializeField] private AudioClip _pause;
    [SerializeField] private AudioClip _newlife;
    [SerializeField] private AudioClip _start;

    private int _currentLives;
    private int _score = 0;
    private int _nextLifeThreshold;

    public int CurrentLives
    {
        get => _currentLives;
        private set
        {
            _currentLives = value;
            UpdateLifeUI();
            if (_currentLives <= 0 && State != StateMachine.Pause)
            {
                GameOver();
            }
        }
    }

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            UpdateScoreText();
            CheckForExtraLife();
        }
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        Score = 0;
        _nextLifeThreshold = bonus;
        State = StateMachine.Play;
        _pauseGUI.enabled = false;
        _gameoverGUI.enabled = false;
        CurrentLives = startingLives;
        PlaySound(_start);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_gameoverGUI.enabled)
            {
                PlaySound(_gameover);
            }
            else
            {
                TogglePause();
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScreen");
        }

        if (State == StateMachine.Pause && _gameoverGUI.enabled && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }

        if (Input.GetKeyUp(KeyCode.Backspace) && !_gameoverGUI.enabled)
        {
            LoseLife();
            BallBehavior.Instance.ResetBall();
        }
    }

    public void LoseLife()
    {
        PlaySound(_loselife);
        CurrentLives--;
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
        _livesGUI.text = "LIFE: " + _currentLives;
    }

    private void CheckForExtraLife()
    {
        while (_score >= _nextLifeThreshold)
        {
            if (CurrentLives < maxLives)
            {
                CurrentLives++;
                PlaySound(_newlife);
            }
            _nextLifeThreshold += bonus;
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    private void UpdateScoreText()
    {
        _scoreGUI.text = "PLAYER\n" + _score;
    }

    private void PlaySound(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
    }
}