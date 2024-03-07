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

        public bool ApplyDamage(float damage, GameObject damageDealer)
        {
            if (_currentHealth <= 0) return false;
            
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die(damageDealer);
                return true;
            }

            return false;
        }
        
        private void Die(GameObject killer)
        {
            if (TryGetComponent(out DestructibleObject destructibleObject))
            {
                destructibleObject.DestroySelf(killer);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}