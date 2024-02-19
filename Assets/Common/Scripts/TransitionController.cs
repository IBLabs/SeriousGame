using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TransitionController : MonoBehaviour
{
    public UnityEvent onEnterTransitionStarted;
    public UnityEvent onEnterTransitionFinished;
    
    public UnityEvent onExitTransitionStarted;
    public UnityEvent onExitTransitionFinished;

    [Header("Dependencies")]
    [SerializeField] private CanvasGroup group;

    [Header("Configuration")]
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private bool fadeOutOnStart = true;
    [SerializeField] private bool nextSceneOnExit = false;
    [SerializeField] private float enterFinishedEventDelay = 0f;

    private void Start()
    {
        if (fadeOutOnStart) StartEnterTransition();
    }

    public void StartEnterTransition()
    {
        StopAllCoroutines();
        StartCoroutine(EnterTransitionCoroutine(duration));
    }

    public void StartExitTransition()
    {
        StopAllCoroutines();
        StartCoroutine(ExitTransitionCoroutine(duration));
    }

    private IEnumerator EnterTransitionCoroutine(float duration)
    {
        onEnterTransitionStarted?.Invoke();
        yield return group.DOFade(0f, duration).From(1f).SetEase(Ease.Linear).WaitForCompletion();
        yield return new WaitForSeconds(enterFinishedEventDelay);
        onEnterTransitionFinished?.Invoke();
    }
    
    private IEnumerator ExitTransitionCoroutine(float duration)
    {
        onExitTransitionStarted?.Invoke();
        yield return group.DOFade(1f, duration).From(0f).SetEase(Ease.Linear).WaitForCompletion();
        onExitTransitionFinished?.Invoke();
        
        if (nextSceneOnExit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
