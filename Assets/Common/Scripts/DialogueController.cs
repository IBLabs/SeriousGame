using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Image dialogueBackground;
    [SerializeField] private CanvasGroup dialogueBoxContainer;
    [SerializeField] private CanvasGroup character;
    [SerializeField] private Animator characterAnimator;

    [Header("Configuration")]
    [SerializeField] private bool showOnStart;
    [SerializeField] private float fadeOutDelay = 1f;

    private void Start()
    {
        SetInitialState();

        if (showOnStart) ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (characterAnimator != null) characterAnimator.SetTrigger("Enter");

        StopAllCoroutines();
        StartCoroutine(SetDialogueVisibleCoroutine(true, 0f));
    }

    public void HideDialogue()
    {
        if (characterAnimator != null) characterAnimator.SetTrigger("Exit");
        
        StopAllCoroutines();
        StartCoroutine(SetDialogueVisibleCoroutine(false, fadeOutDelay));
    }

    private void SetDialogueVisible(bool isVisible)
    {
        dialogueBackground.DOFade(isVisible ? .65f : 0f, .2f).From(isVisible ? .65f : 0f).SetEase(Ease.Linear);
        
        dialogueBoxContainer.DOFade(isVisible ? 1f : 0, .2f).From(isVisible ? 0f : 1f).SetEase(Ease.Linear).SetDelay(.1f);
        if (dialogueBoxContainer.TryGetComponent(out RectTransform dialogueBoxRectTransform))
        {
            dialogueBoxRectTransform.DOMoveY(dialogueBoxRectTransform.position.y + 15 * (isVisible ? 1 : -1), .2f).SetEase(Ease.OutCubic).SetDelay(.1f);
        }
        
        character.DOFade(isVisible ? 1f : 0f, .2f).From(isVisible ? 0f : 1f).SetEase(Ease.Linear).SetDelay(.2f);
        if (character.TryGetComponent(out RectTransform charRectTransform))
        {
            charRectTransform.DOMoveX( charRectTransform.position.x + 15 * (isVisible ? 1 : -1), .2f).SetEase(Ease.OutCubic).SetDelay(.2f);    
        }
    }
    
    private IEnumerator SetDialogueVisibleCoroutine(bool isVisible, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetDialogueVisible(isVisible);
    }

    private void SetInitialState()
    {
        dialogueBackground.color = dialogueBackground.color.WithAlpha(0f);
        dialogueBoxContainer.alpha = 0f;
        character.alpha = 0f;
    }
}
