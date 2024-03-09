using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class RuleSetValidator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip correctAudioClip;
        [SerializeField] private AudioClip wrongAudioClip;
        
        public UnityEvent onRightObjectDestroyed;
        public UnityEvent onWrongObjectDestroyed;
        
        private LevelRuleSet _activeRuleSet;

        public void OnLevelStart(LevelDescriptor levelDescriptor)
        {
            _activeRuleSet = levelDescriptor.ruleSet;
        }

        public void OnObjectDestroyed(DestructibleObject destroyedObject)
        {
            if (destroyedObject.Destroyer.GetComponent<ConveyorEndController>() != null) return;

            bool isValid = ValidateObject(destroyedObject);
            if (isValid)
            {
                PlayWrongSound();
                onWrongObjectDestroyed?.Invoke();
            }
            else
            {
                PlayCorrectSound();
                onRightObjectDestroyed?.Invoke();
            }
        }

        private void PlayCorrectSound()
        {
            if (audioSource != null && correctAudioClip)
            {
                audioSource.PlayOneShot(correctAudioClip);
            }
        }
        
        private void PlayWrongSound()
        {
            if (audioSource != null && wrongAudioClip)
            {
                audioSource.PlayOneShot(wrongAudioClip);
            }
        }
        
        private bool ValidateObject(DestructibleObject destroyedObject)
        {
            if (_activeRuleSet == null) return false;

            bool isSizeValid = false;
            bool isSpotValid = false;
            
            if (destroyedObject.TryGetComponent(out SizeClassable sizeClassable))
            {
                isSizeValid = _activeRuleSet.allowedSizes.Contains(sizeClassable.sizeClass);
            }
            
            if (destroyedObject.TryGetComponent(out Spottalbe spottalbe))
            {
                isSpotValid = _activeRuleSet.spotsAllowed == spottalbe.hasSpots;
            }

            return (isSpotValid && isSizeValid);
        }
    }
}