using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] KeyCode Left;
    [SerializeField] KeyCode Right;
    [SerializeField] private Rigidbody2D _leftFlipper, _rightFlipper;
    HingeJoint hinge;
    JointMotor motor;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().maxAngularVelocity =10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehavior.Instance.State == GameBehavior.StateMachine.Play)

        {
            if (Input.GetKey(Left))
            {
                _leftFlipper.AddTorque(25f);
            }
            else
            {
                _leftFlipper.AddTorque(-20f);
            }

            if (Input.GetKey(Right))
            {
                _rightFlipper.AddTorque(-25f);
            }
            else
            {
                _rightFlipper.AddTorque(20f);
            }
        }
    }
}
