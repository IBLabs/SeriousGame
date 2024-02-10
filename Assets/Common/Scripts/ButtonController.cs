using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform internalButtonTransform;
    [SerializeField] private float buttonYOffset = 0.1f;
    [SerializeField] private float buttonAnimationDuration = 0.2f;

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            AnimateDown();
        } 
        else if (context.phase == InputActionPhase.Canceled)
        {
            AnimateUp();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateDown();        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AnimateUp();
    }

    private void AnimateDown()
    {
        internalButtonTransform
            .DOLocalMoveY(internalButtonTransform.localPosition.y - buttonYOffset, buttonAnimationDuration)
            .SetEase(Ease.OutBack);
    }
    
    private void AnimateUp()
    {
        internalButtonTransform
            .DOLocalMoveY(internalButtonTransform.localPosition.y + buttonYOffset, buttonAnimationDuration)
            .SetEase(Ease.OutBack);
    }
}
