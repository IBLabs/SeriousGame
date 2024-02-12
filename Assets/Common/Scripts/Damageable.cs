using System;
using UnityEngine;

namespace Common.Scripts
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float initialHealth;

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = initialHealth;
        }

        public void ApplyDamage(float damage)
        {
            if (_currentHealth <= 0) return;
            
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}