using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class TouchableObjectNewInput : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayerMask;
        
        private TouchControls touchControls;

        // Declare public Unity Events
        public UnityEvent<Vector2> onTouchBegan;
        public UnityEvent<Vector2> onTouchEnded;
        public UnityEvent<Vector2> onTouchMoved;

        private void Awake()
        {
            touchControls = new TouchControls();
        
            touchControls.Touch.TouchPress.started += ctx => BeginTouch();
            touchControls.Touch.TouchPress.canceled += ctx => EndTouch();
        }

        private void BeginTouch()
        {
            Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
            Debug.Log($"[TEST]: trying to detect touch on object {gameObject.name}");

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayerMask.value))
            {
                if (hit.transform == transform)
                {
                    onTouchBegan?.Invoke(touchPosition);
                    Debug.Log($"Touch started on object {gameObject.name}");    
                }
                else
                {
                    Debug.Log($"[TEST]: touch detected on {hit.transform.name} instead of {gameObject.name}");       
                }
                
            }
        }

        private void EndTouch()
        {
            Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            onTouchEnded?.Invoke(touchPosition);
            Debug.Log("Touch ended");
        }

        private void Update()
        {
            if (touchControls.Touch.TouchPress.ReadValue<float>() != 0)
            {
                Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
                onTouchMoved?.Invoke(touchPosition);
            }
        }

        private void OnEnable()
        {
            touchControls.Enable();
        }

        private void OnDisable()
        {
            touchControls.Disable();
        }
    }
}