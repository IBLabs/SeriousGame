using System;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    public class Spottalbe : MonoBehaviour
    {
        public bool hasSpots;

        public void SetSpotsVisible(bool isVisible)
        {
            if (!TryGetComponent(out MeshRenderer meshRenderer)) return;
            
            foreach (var material in meshRenderer.materials)
            {
                material.SetInt("_SpotsEnabled", isVisible ? 1 : 0);    
            }
            
            hasSpots = isVisible;
        }
    }
}