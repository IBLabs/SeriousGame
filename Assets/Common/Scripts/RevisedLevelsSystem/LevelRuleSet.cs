using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    [CreateAssetMenu(fileName = "LevelRuleSet", menuName = "Level System/Level Rule Set")]
    public class LevelRuleSet: ScriptableObject
    {
        public List<ConveyorObjectSizeClass> allowedSizes;
        public bool spotsAllowed;
    }
}