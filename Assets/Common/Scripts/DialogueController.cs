using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private CanvasGroup dialogueBackground;
    [SerializeField] private CanvasGroup dialogueBoxContainer;
    [SerializeField] private CanvasGroup character;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private GraphicRaycaster raycaster;

    [Header("Configuration")]
    [SerializeField] private bool showOnStart;
    [SerializeField] private float fadeOutDelay = 1f;
    [SerializeField] private float backgroundAlpha = .65f;

    public UnityEvent onDialogueFinishedShow;
    public UnityEvent onDialogueFinishedHide;

    private bool _isGameRunning = true;

    private void Start()
    {
        SetInitialState();

        if (showOnStart) ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (!_isGameRunning) return;

        raycaster.enabled = true;

        if (characterAnimator != null) characterAnimator.SetTrigger("Enter");

        StopAllCoroutines();
        StartCoroutine(SetDialogueVisibleCoroutine(true, 0f));
    }

    public void HideDialogue()
    {
        raycaster.enabled = false;

        if (characterAnimator != null) characterAnimator.SetTrigger("Exit");
        
        StopAllCoroutines();
        StartCoroutine(SetDialogueVisibleCoroutine(false, fadeOutDelay));
    }

    public void OnGameOver()
    {
        _isGameRunning = false;
    }

    private void SetDialogueVisible(bool isVisible)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Join(dialogueBackground.DOFade(isVisible ? .65f : 0f, .2f).From(isVisible ? backgroundAlpha : 0f).SetEase(Ease.Linear));
        
        sequence.Join(dialogueBoxContainer.DOFade(isVisible ? 1f : 0, .2f).From(isVisible ? 0f : 1f).SetEase(Ease.Linear).SetDelay(.1f));
        if (dialogueBoxContainer.TryGetComponent(out RectTransform dialogueBoxRectTransform))
        {
            sequence.Join(dialogueBoxRectTransform.DOMoveY(dialogueBoxRectTransform.position.y + 15 * (isVisible ? 1 : -1), .2f).SetEase(Ease.OutCubic).SetDelay(.1f));
        }
        
        sequence.Join(character.DOFade(isVisible ? 1f : 0f, .2f).From(isVisible ? 0f : 1f).SetEase(Ease.Linear).SetDelay(.2f));
        if (character.TryGetComponent(out RectTransform charRectTransform))
        {
            sequence.Join(charRectTransform.DOMoveX( charRectTransform.position.x + 15 * (isVisible ? 1 : -1), .2f).SetEase(Ease.OutCubic).SetDelay(.2f));    
        }
        
        sequence.onComplete += () =>
        {
            if (isVisible) onDialogueFinishedShow?.Invoke();
            else onDialogueFinishedHide?.Invoke();
        };
    }
    
    private IEnumerator SetDialogueVisibleCoroutine(bool isVisible, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetDialogueVisible(isVisible);
    }

    private void SetInitialState()
    {
        dialogueBackground.alpha = 0;
        dialogueBoxContainer.alpha = 0f;
        character.alpha = 0f;
    }
}
