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
    
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue()
    {
        _index = 0;
        StartCoroutine(TypeLine());
    }

    public void OnNextDialogue(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            NextLine();
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
            dialogueFinished?.Invoke();
        }
    }
}
