using System;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class ConveyorScanner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip scannerAudioClip;
        [SerializeField] private AudioClip correctAudioClip;
        [SerializeField] private AudioClip wrongAudioClip;
        
        public UnityEvent onWrongObjectPassed;

        public UnityEvent onRightObjectPassed;

        private LevelRuleSet _activeRuleSet;

        public void OnLevelStart(LevelDescriptor levelDescriptor)
        {
            _activeRuleSet = levelDescriptor.ruleSet;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ConveyorObject"))
            {
                audioSource.PlayOneShot(scannerAudioClip);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ConveyorObject"))
            {
                if (!CheckAgainstRuleSet(_activeRuleSet, other.gameObject))
                {
                    audioSource.PlayOneShot(wrongAudioClip);
                    onWrongObjectPassed?.Invoke();
                }
                else
                {
                    audioSource.PlayOneShot(correctAudioClip);
                    onRightObjectPassed?.Invoke();
                }
            }
        }

        private bool CheckAgainstRuleSet(LevelRuleSet ruleSet, GameObject targetGameObject)
        {
            if (targetGameObject.TryGetComponent(out SizeClassable sizeClassable))
            {
                if (!ruleSet.allowedSizes.Contains(sizeClassable.sizeClass)) return false;
            }
            
            if (targetGameObject.TryGetComponent(out Spottalbe spottalbe))
            {
                if (!ruleSet.spotsAllowed && spottalbe.hasSpots) return false;
            }

            return true;
        }
    }
}