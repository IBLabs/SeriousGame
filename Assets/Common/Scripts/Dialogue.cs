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
    
    public UnityEvent dialogueFinished;
    public UnityEvent dialogueStarted;

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Segment[] segments;
    [SerializeField] private float textSpeed = .3f;

    private int _segmentIndex;
    private int _lineIndex;

    // Start is called before the first frame update
    private void Start()
    {
        // TODO: remove if not needed
        // StartDialogue();
    }

    public void StartDialogue()
    {
        if (_segmentIndex >= segments.Length) return;

        textComponent.text = string.Empty;
        _lineIndex = 0;

        StartCoroutine(TypeLine());
        
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
        if (textComponent.text == segments[_segmentIndex].lines[_lineIndex])
        {
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
            _segmentIndex++;
            dialogueFinished?.Invoke();
        }
    }
}