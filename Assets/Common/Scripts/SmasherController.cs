using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Scripts
{
    public class SmasherController : MonoBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private float smashSpeed;
        [SerializeField] private float postSmashDelay = .5f;
        [SerializeField] private float returnSpeed;
        [SerializeField] private bool shakeCamera;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float cameraShakeDuration = .2f;
        [SerializeField] private float cameraShakeStrength = 1f;

        [SerializeField] private Transform smashParticleSystem;

        private bool _isSmashing;
        private bool _isColliderActive;

        public void OnSmash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                StartCoroutine(Smash());
            }
        }

        public IEnumerator Smash()
        {
            if (_isSmashing) yield break;
            
            _isSmashing = true;

            _isColliderActive = true;
            
            yield return transform.DOMoveY(transform.position.y + distance, smashSpeed).SetEase(Ease.InQuad).WaitForCompletion();

            if (shakeCamera) targetCamera.DOShakePosition(cameraShakeDuration, cameraShakeStrength);

            _isColliderActive = false;

            yield return transform.DOMoveY(transform.position.y - distance, returnSpeed).SetEase(Ease.OutBack)
                .WaitForCompletion();

            _isSmashing = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isColliderActive) return;

            var position = other.gameObject.transform.position;
            
            Transform newParticleSystem = Instantiate(smashParticleSystem, position, Quaternion.identity);
            
            bool didHit = Physics.Raycast(position, Vector3.down, out RaycastHit hit, 10f);
            if (didHit)
            {
                newParticleSystem.position = hit.point;
            }

            Destroy(other.gameObject);
        }
    }
}