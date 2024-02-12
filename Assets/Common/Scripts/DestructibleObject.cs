using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class DestructibleObject : MonoBehaviour
    {
        [System.Serializable]
        public class ObjectDestroyedEvent : UnityEvent<DestructibleObject> {}

        [SerializeField] public ObjectDestroyedEvent onObjectDestroyed;
        public LayerMask floorLayerMask;

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