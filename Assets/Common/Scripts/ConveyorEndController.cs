using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class ConveyorEndController : MonoBehaviour
    {
        public UnityEvent objectDestroyed;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ConveyorObject"))
            {
                Destroy(other.gameObject);
                objectDestroyed?.Invoke();
            }
        }
    }
}