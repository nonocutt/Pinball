using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehavior : MonoBehaviour
{
    public Transform spring;
    public Transform ball;
    public float maxStretch = 5f;
    public float launchForce = 7f;
    private Vector3 initialPosition;
    private bool isPressing = false;
    private float currentStretch = 0f;
    public float stretchSpeed = 1f;
    public float activationDistance = 1f; // Distance within which the ball can activate the spring

    void Start()
    {
        initialPosition = spring.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isPressing = true;
            currentStretch += stretchSpeed * Time.deltaTime;
            if (currentStretch > maxStretch)
            {
                currentStretch = maxStretch;
            }
            spring.position = initialPosition - new Vector3(0, currentStretch, 0);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isPressing)
        {
            isPressing = false;
            Vector3 direction = initialPosition - spring.position;

            // Check if the ball is within activation distance
            if (ball != null && Vector3.Distance(ball.position, initialPosition) <= activationDistance)
            {
                Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();
                if (ballRigidbody != null)
                {
                    ballRigidbody.AddForce(direction * launchForce, ForceMode2D.Impulse);
                }
            }

            ResetSpring();
        }
    }

    void ResetSpring()
    {
        spring.position = initialPosition;
        currentStretch = 0f;
    }
}