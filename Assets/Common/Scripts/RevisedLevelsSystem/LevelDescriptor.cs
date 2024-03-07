using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Scripts.RevisedLevelsSystem
{
    [CreateAssetMenu(fileName = "LevelDescriptor", menuName = "Level System/Level Descriptor")]
    public class LevelDescriptor: ScriptableObject
    {
        public int amountToSpawn = 0;
        public LevelRuleSet ruleSet;
        public float spawnRate = 1.0f;
        public float validObjectsPercentage = .5f;
        
        public bool IsSizeClassValid(ConveyorObjectSizeClass sizeClass)
        {
            return ruleSet.allowedSizes.Contains(sizeClass);
        }
    }
}