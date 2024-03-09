using System;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class ConveyorEndController : MonoBehaviour
    {
        [SerializeField] private Transform teleportTarget;
        
        public UnityEvent objectDestroyed;

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ConveyorObject"))
            {
                if (other.TryGetComponent(out DestructibleObject destructibleObject))
                {
                    destructibleObject.DestroySelf(gameObject);
                    // other.attachedRigidbody.MovePosition(teleportTarget.position);
                }
                else
                {
                    Destroy(other.gameObject);
                }
                
                objectDestroyed?.Invoke();
            }
        }
    }
}