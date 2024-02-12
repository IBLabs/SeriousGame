using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class TouchableObjectNewInput : MonoBehaviour
    {
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
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                onTouchBegan?.Invoke(touchPosition); // Invoke the onTouchBegan event
                Debug.Log("Touch started on object");
            }
        }

        private void EndTouch()
        {
            Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            onTouchEnded?.Invoke(touchPosition); // Invoke the onTouchEnded event
            Debug.Log("Touch ended");
        }

        private void Update()
        {
            if (touchControls.Touch.TouchPress.ReadValue<float>() != 0)
            {
                // If there's an ongoing touch, track the touch position
                Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
                onTouchMoved?.Invoke(touchPosition); // Invoke the onTouchMoved event
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