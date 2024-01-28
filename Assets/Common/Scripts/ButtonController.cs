using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Transform internalButtonTransform;
    [SerializeField] private float buttonYOffset = 0.1f;
    [SerializeField] private float buttonAnimationDuration = 0.2f;

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            internalButtonTransform
                .DOLocalMoveY(internalButtonTransform.localPosition.y - buttonYOffset, buttonAnimationDuration)
                .SetEase(Ease.OutBack);
        } 
        else if (context.phase == InputActionPhase.Canceled)
        {
            internalButtonTransform
                .DOLocalMoveY(internalButtonTransform.localPosition.y + buttonYOffset, buttonAnimationDuration)
                .SetEase(Ease.OutBack);
        }
    }
}
