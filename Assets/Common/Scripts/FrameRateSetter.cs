using System;
using UnityEngine;

namespace Common.Scripts
{
    public class FrameRateSetter : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}