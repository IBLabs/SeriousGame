using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Dialogue : MonoBehaviour
{
    public UnityEvent dialogueFinished;

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed = .3f;

    private int _index;
    private bool _isDialogueFinished;

    // Start is called before the first frame update
    private void Start()
    {
        // TODO: remove if not needed
        // StartDialogue();
    }

    public void StartDialogue()
    {
        textComponent.text = string.Empty;
        _index = 0;

        StartCoroutine(TypeLine());
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
        if (_isDialogueFinished) return;

        if (textComponent.text == lines[_index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[_index];
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in lines[_index].ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (_index < lines.Length - 1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            _isDialogueFinished = true;
            dialogueFinished?.Invoke();
        }
    }
}