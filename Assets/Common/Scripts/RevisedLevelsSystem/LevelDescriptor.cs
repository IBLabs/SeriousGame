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
        [FormerlySerializedAs("spawnRate")] public float VegetablesPerSecond = 1.0f;
        public SpawnConfiguration spawnConfiguration;
    }
}