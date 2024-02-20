using System;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    [CreateAssetMenu(fileName = "LevelDescriptor", menuName = "Level System/Level Descriptor")]
    public class LevelDescriptor: ScriptableObject
    {
        public int amountToSpawn = 0;
        public LevelRuleSet ruleSet;
        public float spawnRate = 1.0f;
        public SpawnConfiguration spawnConfiguration;
    }
}