using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] private float _speed = 5f; // Default speed value for the ball
    [SerializeField] private float bounceForce = 1f; // Bounce force to maintain the current speed
    private Vector2 _direction; // Direction vector for ball movement
    private Rigidbody2D _rb; // Reference to the Rigidbody2D component

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(1, 1);
        ResetBall(); // Initialize the ball position and movement
    }

    void Update()
    {
        // Check if the game is in the "Play" state
        if (GameBehavior.Instance.State == GameBehavior.StateMachine.Play)
        {
            _rb.velocity = _rb.velocity.normalized * _speed; // Maintain the ball's speed
        }
    }

    void ResetBall()
    {
        transform.position = Vector3.zero; // Reset the ball position to the center

        // Randomize initial direction and normalize it
        _direction = new Vector2(
            Random.value > 0.5f ? 1 : -1, // Randomize x direction (left or right)
            Random.value > 0.5f ? 1 : -1  // Randomize y direction (up or down)
        ).normalized; // Normalize the direction vector

        // Apply initial velocity to the ball
        _rb.velocity = _direction * _speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is tagged as "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Get the normal of the collision
            Vector2 normal = collision.contacts[0].normal;
            // Reflect the current velocity based on the collision normal
            Vector2 newVelocity = Vector2.Reflect(_rb.velocity, normal);
            // Apply the new velocity to the Rigidbody2D component
            _rb.velocity = newVelocity;
            Debug.Log("Wall hit");
        }
    }
}