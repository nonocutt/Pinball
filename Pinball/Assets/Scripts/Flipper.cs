using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] KeyCode Left;
    [SerializeField] KeyCode Right;
    [SerializeField] private Rigidbody2D _leftFlipper;
    [SerializeField] private Rigidbody2D _rightFlipper;
    [SerializeField] private float leftFlipperMaxAngle = 45f; // Maximum rotation angle for left flipper
    [SerializeField] private float rightFlipperMaxAngle = -45f; // Maximum rotation angle for right flipper
    [SerializeField] private float flipperSpeed = 20000f; // Speed of rotation in degrees per second

    private float leftFlipperInitialAngle;
    private float rightFlipperInitialAngle;

    void Start()
    {
        // Set the rigidbodies to Kinematic
        _leftFlipper.bodyType = RigidbodyType2D.Kinematic;
        _rightFlipper.bodyType = RigidbodyType2D.Kinematic;

        // Store the initial angles of the flippers
        leftFlipperInitialAngle = _leftFlipper.rotation;
        rightFlipperInitialAngle = _rightFlipper.rotation;
    }

    void Update()
    {
        if (GameBehavior.Instance.State == GameBehavior.StateMachine.Play)
        {
            HandleFlipperRotation(_leftFlipper, Left, leftFlipperInitialAngle, leftFlipperMaxAngle);
            HandleFlipperRotation(_rightFlipper, Right, rightFlipperInitialAngle, rightFlipperMaxAngle);
        }
    }

    private void HandleFlipperRotation(Rigidbody2D flipper, KeyCode key, float initialAngle, float maxAngle)
    {
        float targetAngle = initialAngle;
        if (Input.GetKey(key))
        {
            targetAngle = maxAngle;
        }

        // Gradually rotate the flipper towards the target angle
        float currentAngle = flipper.rotation;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, flipperSpeed * Time.deltaTime);
        flipper.MoveRotation(newAngle);
    }
}