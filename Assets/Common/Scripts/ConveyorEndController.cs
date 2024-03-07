using System;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class ConveyorEndController : MonoBehaviour
    {
        public UnityEvent wrongObjectPassed;
        public UnityEvent objectDestroyed;

        private LevelRuleSet _activeRuleSet;

        public void OnLevelStart(LevelDescriptor levelDescriptor)
        {
            _activeRuleSet = levelDescriptor.ruleSet;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ConveyorObject"))
            {
                if (!CheckAgainstRuleSet(_activeRuleSet, other.gameObject))
                {
                    Debug.Log("[TEST]: wrong object entered!");
                    wrongObjectPassed?.Invoke();
                }

                if (other.TryGetComponent(out DestructibleObject destructibleObject))
                {
                    destructibleObject.DestroySelf(gameObject);
                }
                else
                {
                    Destroy(other.gameObject);    
                }
                
                objectDestroyed?.Invoke();
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