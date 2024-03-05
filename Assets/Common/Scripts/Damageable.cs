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

        public bool ApplyDamage(float damage)
        {
            if (_currentHealth <= 0) return false;
            
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die();
                return true;
            }

            return false;
        }
        
        private void Die()
        {
            if (TryGetComponent(out DestructibleObject destructibleObject))
            {
                destructibleObject.DestroySelf();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}