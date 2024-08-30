using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;

    private bool _hasBeenLaunced;
    private bool _shouldFaceVelocityDirection;

    private void Awake()
    {
        // get AngryBird rigidbody & circle collider
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        // Body Type: Kinematic - Use kinematic body type to design rigidbody 2D to move under simulation only with explicit user control
        _rigidbody.isKinematic = true;
        _circleCollider.enabled = false;
    }

    // called 50 times a second
    private void FixedUpdate()
    {
        if (_hasBeenLaunced && _shouldFaceVelocityDirection)
        {
            // make the transform face the velocity direction
            transform.right = _rigidbody.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _shouldFaceVelocityDirection = false;
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        _shouldFaceVelocityDirection = true;

        _rigidbody.isKinematic = false;
        _circleCollider.enabled = true;

        // apply force - .Impulse instantly adds the force, and not over time ramps up
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);

        _hasBeenLaunced = true;
    }
}
