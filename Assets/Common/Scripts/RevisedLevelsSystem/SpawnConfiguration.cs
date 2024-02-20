using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    //add menu
    [CreateAssetMenu(fileName = "SpawnConfiguration", menuName = "Level System/Spawn Configuration")]
    public class SpawnConfiguration: ScriptableObject
    {
        public List<ConveyorObjectSizeClass> sizeClasses;
        public bool spotsEnabled;
    }
}