using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class TriggerCollisionUnityEvent : MonoBehaviour
    {
        public UnityEvent onTriggerCollisionEnter;
        public UnityEvent onTriggerCollisionExit;
        
        private void OnTriggerEnter(Collider other)
        {
            onTriggerCollisionEnter?.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            onTriggerCollisionExit?.Invoke();
        }
    }
}