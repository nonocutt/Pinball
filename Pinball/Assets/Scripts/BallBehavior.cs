using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallBehavior : MonoBehaviour
{
    public static BallBehavior Instance;

    [SerializeField] KeyCode LN;
    [SerializeField] KeyCode RN;
    
    private Rigidbody2D rb;
    [SerializeField] private float _yLimit = 8.0f;
    private Vector3 initialPosition;
    private CoinBehavior[] coins;
    private bool isBigBallActive = false;
    public bool isDoubleScoreActive = false;
    private bool isWallHitBonusActive = false;

    private readonly float[] pitchMultipliers = { 0.75f, 0.842f, 0.89f, 1f, 1.1225f, 1.26f, 1.335f, 1.5f };
    
    private AudioSource _source;

    [Space(4)] [Header("Audio")] [SerializeField]
    private AudioClip _wallhit;

    [SerializeField] private AudioClip _flipperhit;
    [SerializeField] private AudioClip _bumperhit;
    [SerializeField] private AudioClip _tunnel;
    [SerializeField] private AudioClip _launch;
    [SerializeField] private AudioClip _coin;
    [SerializeField] private AudioClip _500;
    [SerializeField] private AudioClip _nudgeSound;
    [SerializeField] private AudioClip _powerup;

    [Space(4)] [Header("Nudge Settings")] [SerializeField]
    private float nudgeForce = 1.0f;

    [SerializeField] private float nudgeCooldown = 1.0f;
    private float lastNudgeTime;

    // Names of slingshots
    private string[] slingshotNames = { "slingshotL", "slingshotR", "slingshotBumpL", "slingshotBumpR" };
    private GameObject[] slingshots;

    private Color defaultColor = new Color(0.894f, 1.0f, 0.494f); // E4FF7E
    private SpriteRenderer spriteRenderer;

    private Coroutine colorChangeCoroutine;

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Save the initial position for resetting the ball
        initialPosition = transform.position;
        // Find all coin instances in the scene
        coins = FindObjectsOfType<CoinBehavior>();
        // Find all slingshot instances by names
        slingshots = FindSlingshotsByName();
    }

    private void Start()
    {
        ResetBall();
    }

    private void FixedUpdate()
    {
        // Check if the game is in the play state
        if (GameBehavior.Instance.State == GameBehavior.StateMachine.Pause)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = isBigBallActive ? 0.85f : 1.2f;
        }

        // Check if the ball exceeds the y-axis limit
        if (Mathf.Abs(transform.position.y) > _yLimit)
        {
            GameBehavior.Instance.LoseLife();
            ResetBall();
        }
    }

    private void Update()
    {


        if (GameBehavior.Instance.State == GameBehavior.StateMachine.Play)
        {
            HandleNudge();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bumper":
                PlaySound(_bumperhit);
                int score100 = isDoubleScoreActive ? 200 : 100;
                GameBehavior.Instance.AddScore(score100);
                break;
            case "Wall":
                PlaySound(_wallhit);
                if (isWallHitBonusActive)
                {
                    GameBehavior.Instance.AddScore(10);
                }

                break;
            case "Flipper":
                PlaySound(_flipperhit);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "200":
                PlaySound(_tunnel);
                int score200 = isDoubleScoreActive ? 400 : 200;
                GameBehavior.Instance.AddScore(score200);
                break;
            case "300":
                PlaySound(_tunnel);
                int score300 = isDoubleScoreActive ? 600 : 300;
                GameBehavior.Instance.AddScore(score300);
                break;
            case "500":
                PlaySound(_500);
                int score500 = isDoubleScoreActive ? 1000 : 500;
                GameBehavior.Instance.AddScore(score500);
                break;
            case "StartWall":
                PlaySound(_launch);
                break;
            case "Coin":
                PlaySound(_coin);
                break;
            case "BigBall":
                ActivateBigBall();
                break;
            case "DoubleScore":
                ActivateDoubleScore();
                break;
        }
    }

    public void ResetBall()
    {
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1.2f;
        RespawnAllCoins();
        isBigBallActive = false;
        isDoubleScoreActive = false;
        isWallHitBonusActive = false;
        transform.localScale = Vector3.one * 0.3f;
        spriteRenderer.color = defaultColor; // Reset color
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = null;
        }

        ShowSlingshots();
    }

    void RespawnAllCoins()
    {
        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(true); // Reactivate each coin
            StopCoroutine(coin.RespawnCoin()); // Stop the respawn coroutine if it's running
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == _wallhit)
        {
            _source.pitch = pitchMultipliers[Random.Range(0, pitchMultipliers.Length)];
        }
        else
        {
            _source.pitch = 1.0f;
        }
        _source.clip = clip;
        _source.PlayOneShot(clip);
    }

    public void ActivateBigBall()
    {
        if (!isBigBallActive)
        {
            StartCoroutine(BigBallRoutine());
            isWallHitBonusActive = true;
            PlaySound(_powerup);
        }
    }

    private IEnumerator BigBallRoutine()
    {
        isBigBallActive = true;
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.5f; // Increase the ball size by 50%
        rb.gravityScale = 0.85f; // Change gravity scale to 0.85
        HideSlingshots(); // Hide slingshots
        yield return new WaitForSeconds(10f); // Big ball effect lasts for 10 seconds
        transform.localScale = originalScale; // Revert to original size
        rb.gravityScale = 1.2f; // Revert gravity scale
        ShowSlingshots(); // Show slingshots
        isBigBallActive = false;
        isWallHitBonusActive = false;
    }

    public void ActivateDoubleScore()
    {
        StartCoroutine(DoubleScoreRoutine());
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
            PlaySound(_powerup);
        }

        colorChangeCoroutine = StartCoroutine(ColorChangeRoutine(10, 3));
    }

    private IEnumerator DoubleScoreRoutine()
    {
        isDoubleScoreActive = true;
        yield return new WaitForSeconds(10f); // Double score lasts for 10 seconds
        isDoubleScoreActive = false;
        spriteRenderer.color = defaultColor; // Reset color
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = null;
        }
    }

    private void HideSlingshots()
    {
        foreach (var slingshot in slingshots)
        {
            slingshot.SetActive(false);
        }
    }

    private void ShowSlingshots()
    {
        foreach (var slingshot in slingshots)
        {
            slingshot.SetActive(true);
        }
    }

    private GameObject[] FindSlingshotsByName()
    {
        var foundSlingshots = new List<GameObject>();
        foreach (var name in slingshotNames)
        {
            var slingshot = GameObject.Find(name);
            if (slingshot != null)
            {
                foundSlingshots.Add(slingshot);
            }
        }

        return foundSlingshots.ToArray();
    }

    private IEnumerator ColorChangeRoutine(int duration, int slowDuration)
    {
        float endTime = Time.time + duration;
        float slowStartTime = endTime - slowDuration;

        while (Time.time < endTime)
        {
            float waitTime = Time.time >= slowStartTime ? 0.25f : 0.1f; // 4 times per second or 10 times per second
            spriteRenderer.color = new Color(
                Random.Range(0.5f, 0.88f), // 128/255 to 224/255
                Random.Range(0.5f, 0.88f),
                Random.Range(0.5f, 0.88f)
            );
            yield return new WaitForSeconds(waitTime);
        }

        spriteRenderer.color = defaultColor; // Reset color when the routine ends
    }

    private void HandleNudge()
    {
        // Nudge left
        if (Input.GetKeyDown(LN))
        {
            Nudge(Vector2.left);
        }

        // Nudge right
        if (Input.GetKeyDown(RN))
        {
            Nudge(Vector2.right);
        }
    }

    private void Nudge(Vector2 direction)
    {
        if (Time.time - lastNudgeTime > nudgeCooldown)
        {
            rb.AddForce(direction * nudgeForce, ForceMode2D.Impulse);
            lastNudgeTime = Time.time;
            PlaySound(_nudgeSound);
        }
    }
}