using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Common.Scripts
{
    public class DestructibleObject : MonoBehaviour
    {
        [System.Serializable]
        public class ObjectDestroyedEvent : UnityEvent<DestructibleObject> {}

        public bool IsBeingDestroyed { get; private set; }

        [SerializeField] public ObjectDestroyedEvent onObjectDestroyed = new();
        [SerializeField] public ObjectDestroyedEvent onMarkedForDestroy = new();
        public LayerMask floorLayerMask;

        public void DestroySelf()
        {
            IsBeingDestroyed = true;
            onMarkedForDestroy?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            onObjectDestroyed?.Invoke(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & floorLayerMask) != 0)
            {
                onObjectDestroyed?.Invoke(this);
            }
        }
    }
}