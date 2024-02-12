using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchableObject : MonoBehaviour
{
    public UnityEvent<Vector2> touchBegan;
    public UnityEvent<Vector2> touchMoved;
    public UnityEvent<Vector2> touchEnded;

    [SerializeField] private Camera targetCamera;
    
    private bool isTouched = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = targetCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                    {
                        isTouched = true;
                        touchBegan?.Invoke(touch.position);
                    }
                    break;

                case TouchPhase.Moved:
                    if (isTouched)
                    {
                        touchMoved?.Invoke(touch.position);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isTouched)
                    {
                        isTouched = false;
                        touchEnded?.Invoke(touch.position);
                    }
                    break;
            }
        }
    }
}
