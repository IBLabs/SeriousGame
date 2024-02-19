using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Image dialogueBackground;
    [SerializeField] private CanvasGroup dialogueBoxContainer;
    [SerializeField] private CanvasGroup character;

    private void Start()
    {
        // ShowDialogue();
    }

    public void ShowDialogue()
    {
        SetDialogueVisible(true);
    }

    public void HideDialogue()
    {
        SetDialogueVisible(false);
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
}
