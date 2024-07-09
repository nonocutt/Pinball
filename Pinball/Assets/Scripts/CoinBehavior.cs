using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CoinBehavior : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private static List<CoinBehavior> allCoins = new List<CoinBehavior>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        allCoins.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameBehavior.Instance.AddScore(50); // Assuming you have a ScoreManager
            StartCoroutine(RespawnCoin());
            DisableCoin(); // Deactivate the coin
        }
    }

    private void DisableCoin()
    {
        spriteRenderer.enabled = false;
        collider2D.enabled = false;
    }

    private void EnableCoin()
    {
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
    }

    public IEnumerator RespawnCoin()
    {
        yield return new WaitForSeconds(10f); // Wait for 10 seconds
        EnableCoin(); // Reactivate the coin
    }

    public static void RespawnAllCoins()
    {
        foreach (var coin in allCoins)
        {
            coin.StopAllCoroutines();
            coin.EnableCoin();
        }
    }
}