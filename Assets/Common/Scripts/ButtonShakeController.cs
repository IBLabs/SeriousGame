using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonShakeController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private Tweener _shakeTweener;

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _shakeTweener = targetTransform.DOShakePosition(50f, .3f);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _shakeTweener.Kill();
        }
    }
}
