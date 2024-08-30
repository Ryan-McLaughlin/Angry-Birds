using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3;
    [SerializeField] private float _damageThreshold = 0.75f;
    [SerializeField] private GameObject _enemyDeathParticle;

    private float _currentHealth;

    private void Awake()
    {
        Debug.Log("Enemy.Awake()");
        _currentHealth = _maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy.OnCollisionEnter2D()");// Collision2D: " + collision);
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > _damageThreshold)
        {
            DamageEnemy(impactVelocity);
        }
    }

    public void DamageEnemy(float damageAmount)
    {
        Debug.Log("Enemy.DamageEnemy() damageAmount: " + damageAmount);
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // call game manager remove enemy method
        GameManager.Instance.RemoveEnemy(this);

        // generate the death particle
        Instantiate(_enemyDeathParticle, transform.position, Quaternion.identity);

        // destroy this enemy
        Destroy(gameObject);
    }
}
