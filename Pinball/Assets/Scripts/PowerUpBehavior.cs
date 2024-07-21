using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public enum PowerUpType
    {
        DoubleScore,
        BigBall
    }

    public PowerUpType powerUpType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameBehavior.Instance.OnPowerUpCollected(powerUpType);
            Destroy(gameObject); // Destroy power-up after collection
        }
    }
}