using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    private AudioSource _source;
    [SerializeField] private AudioSource _backgroundMusicSource; // Reference to the background music AudioSource

    public enum StateMachine
    {
        Play, // 0
        Pause // 1
    }

    public StateMachine State { get; private set; }

    [SerializeField] private GameObject _pauseGUI; // Changed to GameObject
    [SerializeField] private GameObject _gameoverGUI; // Changed to GameObject
    [SerializeField] private TextMeshProUGUI _livesGUI;
    [SerializeField] private TextMeshProUGUI _scoreGUI;
    [SerializeField] private TextMeshProUGUI _highScoreGUI;
    [SerializeField] private TextMeshProUGUI _newHighScoreGUI; // New High Score Text
    [SerializeField] private GameObject _helpGUI; // Changed to GameObject
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int maxLives = 9;
    [SerializeField] private int bonus = 10000;
    [SerializeField] private AudioClip _loselife;
    [SerializeField] private AudioClip _gameover;
    [SerializeField] private AudioClip _pause;
    [SerializeField] private AudioClip _select;
    [SerializeField] private AudioClip _newlife;
    [SerializeField] private AudioClip _start;
    [SerializeField] private GameObject BigBallPowerUpPrefab;
    [SerializeField] private GameObject doubleScorePowerUpPrefab;

    private int _currentLives;
    private int _score = 0;
    private int _nextLifeThreshold;
    private GameObject currentPowerUpItem;

    private const string HighScoreKey = "HighScore";

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
        StartCoroutine(PowerUpSpawner());
    }

    private void ResetGame()
    {
        Score = 0;
        _nextLifeThreshold = bonus;
        State = StateMachine.Play;
        _pauseGUI.SetActive(false); // Use SetActive instead of enabled
        _gameoverGUI.SetActive(false); // Use SetActive instead of enabled
        _helpGUI.SetActive(false); // Ensure help GUI is hidden initially
        _newHighScoreGUI.gameObject.SetActive(false); // Ensure new high score text is hidden initially
        CurrentLives = startingLives;
        BallBehavior.Instance.ResetBall();
        PlaySound(_start);
        UpdateHighScoreUI();
        SetBackgroundMusicVolume(0.35f);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (State == StateMachine.Play)
            {
                TogglePause();
            }
            else if (State == StateMachine.Pause && !_gameoverGUI.activeSelf && !_helpGUI.activeSelf)
            {
                TogglePause();
            }
            else if (_helpGUI.activeSelf)
            {
                ToggleHelp();
            }
        }

        if (State == StateMachine.Pause && _gameoverGUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }

        if (State == StateMachine.Pause && !_gameoverGUI.activeSelf && !_helpGUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                TogglePause();
                SceneManager.LoadScene("TitleScreen");
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleHelp();
            }
        }

        //if (Input.GetKeyUp(KeyCode.Backspace) && !_gameoverGUI.activeSelf)
        //{
        //    LoseLife();
        //    BallBehavior.Instance.ResetBall();
        //}
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
        _pauseGUI.SetActive(!_pauseGUI.activeSelf); // Use SetActive to toggle
        Time.timeScale = State == StateMachine.Pause ? 0 : 1; // Pause or resume time
        SetBackgroundMusicVolume(State == StateMachine.Pause ? 0.08f : 0.35f); // Dim or restore background music volume
    }

    private void ToggleHelp()
    {
        _helpGUI.SetActive(!_helpGUI.activeSelf); // Toggle help GUI
        PlaySound(_select);
    }

    private void GameOver()
    {
        PlaySound(_gameover);
        State = StateMachine.Pause;
        _gameoverGUI.SetActive(true);
        _pauseGUI.SetActive(false);
        CheckAndSaveHighScore();
        SetBackgroundMusicVolume(0.08f);
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

    private IEnumerator PowerUpSpawner()
    {
        while (true)
        {
            if (currentPowerUpItem == null)
            {
                yield return new WaitForSeconds(30f);
                SpawnRandomPowerUp();
            }

            yield return null;
        }
    }

    private void SpawnRandomPowerUp()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1f, 1.5f), 0f); // Adjust as necessary
        GameObject powerUpPrefab = Random.value > 0.5f ? BigBallPowerUpPrefab : doubleScorePowerUpPrefab;
        currentPowerUpItem = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        PowerUpBehavior powerUpBehavior = currentPowerUpItem.GetComponent<PowerUpBehavior>();
    }

    public void OnPowerUpCollected(PowerUpBehavior.PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpBehavior.PowerUpType.BigBall:
                BallBehavior.Instance.ActivateBigBall();
                break;
            case PowerUpBehavior.PowerUpType.DoubleScore:
                BallBehavior.Instance.ActivateDoubleScore();
                break;
        }

        currentPowerUpItem = null;
    }

    private void CheckAndSaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (_score > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, _score);
            PlayerPrefs.Save();
            _newHighScoreGUI.gameObject.SetActive(true);
            UpdateHighScoreUI();
        }
    }

    private void UpdateHighScoreUI()
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        _highScoreGUI.text = "HI SCORE\n" + highScore;
    }

    private void SetBackgroundMusicVolume(float volume)
    {
        if (_backgroundMusicSource != null)
        {
            _backgroundMusicSource.volume = volume;
        }
    }
}