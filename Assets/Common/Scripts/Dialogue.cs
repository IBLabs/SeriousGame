using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class Segment
    {
        public string[] lines;
    }

    public UnityEvent dialogueStarted;
    public UnityEvent dialogueFinished;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip nextLineAudioClip;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Segment[] segments;
    [SerializeField] private float textSpeed = .3f;

    [Header("Configuration")]
    [SerializeField] private bool playOnStart;

    private int _segmentIndex;
    private int _lineIndex;
    private bool _isDialogueActive;

    // Start is called before the first frame update
    private void Start()
    {
        if (playOnStart) StartDialogue();
    }

    public void StartDialogue()
    {
        if (_segmentIndex >= segments.Length) return;

        textComponent.text = string.Empty;
        _lineIndex = 0;

        StartCoroutine(TypeLine());
        
        _isDialogueActive = true;
        
        dialogueStarted?.Invoke();
    }

    public void OnNextDialogue(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        if (!_isDialogueActive) return;

        if (textComponent.text == segments[_segmentIndex].lines[_lineIndex])
        {
            _audioSource.PlayOneShot(nextLineAudioClip);
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = segments[_segmentIndex].lines[_lineIndex];
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in segments[_segmentIndex].lines[_lineIndex].ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (_lineIndex < segments[_segmentIndex].lines.Length - 1)
        {
            _lineIndex++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (_segmentIndex < segments.Length - 1)
            {
                _segmentIndex++;
            }

            _isDialogueActive = false;

            dialogueFinished?.Invoke();
        }
    }
}