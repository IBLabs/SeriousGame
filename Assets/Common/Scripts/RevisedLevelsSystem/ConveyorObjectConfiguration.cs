using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    [CreateAssetMenu(fileName = "ConveyorObjectConfiguration", menuName = "Level System/Conveyor Object Configuration")]
    public class ConveyorObjectConfiguration : ScriptableObject
    {
        public List<SizeClassWithPrefab> prefabs;
        
        public GameObject GetPrefab(ConveyorObjectSizeClass sizeClass)
        {
            foreach (var sizeClassWithPrefab in prefabs)
            {
                if (sizeClassWithPrefab.sizeClass == sizeClass)
                {
                    return sizeClassWithPrefab.prefab;
                }
            }

            return null;
        }
    
        [Serializable]
        public class SizeClassWithPrefab
        {
            public ConveyorObjectSizeClass sizeClass;
            public GameObject prefab;
        }
    }
}