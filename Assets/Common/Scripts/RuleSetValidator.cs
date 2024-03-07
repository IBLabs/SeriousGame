using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;

namespace Common.Scripts
{
    public class RuleSetValidator : MonoBehaviour
    {
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
                OnRightObjectDestroyed();
            }
            else
            {
                OnWrongObjectDestroyed();
            }
        }
        
        private bool ValidateObject(DestructibleObject destroyedObject)
        {
            if (_activeRuleSet == null) return false;
            
            if (destroyedObject.TryGetComponent(out SizeClassable sizeClassable))
            {
                bool isSizeValid = _activeRuleSet.allowedSizes.Contains(sizeClassable.sizeClass);
                if (isSizeValid) return false;
            }
            
            if (destroyedObject.TryGetComponent(out Spottalbe spottalbe))
            {
                bool isSpotValid = _activeRuleSet.spotsAllowed == spottalbe.hasSpots;
                if (isSpotValid) return false;
            }

            return true;
        }

        private void OnWrongObjectDestroyed()
        {
            
        }

        private void OnRightObjectDestroyed()
        {
            
        }
    }
}