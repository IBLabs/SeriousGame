using System;
using UnityEngine;

namespace Common.Scripts
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 1f;
        
        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}