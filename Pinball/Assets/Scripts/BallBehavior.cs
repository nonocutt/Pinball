using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallBehavior : MonoBehaviour
{
    public static BallBehavior Instance;
    
    private Rigidbody2D rb;
    [SerializeField] private float _yLimit = 8.0f;
    private Vector3 initialPosition;
    private CoinBehavior[] coins;
    
    private AudioSource _source;
    [Space(4)]
    [Header("Audio")]
    [SerializeField] private AudioClip _wallhit;
    [SerializeField] private AudioClip _flipperhit;
    [SerializeField] private AudioClip _bumperhit;
    [SerializeField] private AudioClip _tunnel;
    [SerializeField] private AudioClip _launch;
    [SerializeField] private AudioClip _coin;
    [SerializeField] private AudioClip _500;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();

        // Ensure the Rigidbody2D's material is set to a bouncy material
        var collider = GetComponent<Collider2D>();
        if (collider != null && collider.sharedMaterial == null)
        {
            Debug.LogWarning("No Physics Material assigned to the Collider2D. Please assign a bouncy material.");
        }

        // Save the initial position for resetting the ball
        initialPosition = transform.position;
        // Find all coin instances in the scene
        coins = FindObjectsOfType<CoinBehavior>();
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
            rb.gravityScale = 1.2f; // Set to a suitable gravity scale for your game
        }

        // Check if the ball exceeds the y-axis limit
        if (Mathf.Abs(transform.position.y) > _yLimit)
        {
            GameBehavior.Instance.LoseLife();
            ResetBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bumper":
                PlaySound(_bumperhit);
                GameBehavior.Instance.AddScore(100);
                break;
            case "Wall":
                PlaySound(_wallhit);
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
                GameBehavior.Instance.AddScore(200);
                break;
            case "300":
                PlaySound(_tunnel);
                GameBehavior.Instance.AddScore(300);
                break;
            case "500":
                PlaySound(_500);
                GameBehavior.Instance.AddScore(500);
                break;
            case "StartWall":
                PlaySound(_launch);
                break;
            case "Coin":
                PlaySound(_coin);
                break;
        }
    }

    public void ResetBall()
    {
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1.2f;
        RespawnAllCoins();
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
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.clip = clip;
        _source.PlayOneShot(clip);
    }
}