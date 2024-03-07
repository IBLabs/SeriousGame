using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Common.Scripts
{
    public class SmasherController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private float distance;
        [SerializeField] private float smashSpeed;
        [SerializeField] private float returnSpeed;
        [SerializeField] private bool shakeCamera;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float cameraShakeDuration = .2f;
        [SerializeField] private float cameraShakeStrength = 1f;
        [SerializeField] private LayerMask conveyorBeltLayerMask;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip smashSound;

        [SerializeField] private Transform smashParticleSystem;

        private bool _isSmashing;
        private bool _isColliderActive;

        public void OnTouchBegin(Vector2 touchPosition)
        {
            StartCoroutine(Smash());
        }

        public void OnSmash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                StartCoroutine(Smash());
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            StartCoroutine(Smash());
        }

        public IEnumerator Smash()
        {
            if (_isSmashing) yield break;

            _isSmashing = true;

            // _isColliderActive = true;
            
            audioSource.PlayOneShot(smashSound);

            yield return transform.DOMoveY(transform.position.y + distance, smashSpeed).SetEase(Ease.InQuad)
                .WaitForCompletion();

            if (shakeCamera) targetCamera.DOShakePosition(cameraShakeDuration, cameraShakeStrength);

            // _isColliderActive = false;

            yield return transform.DOMoveY(transform.position.y - distance, returnSpeed).SetEase(Ease.OutBack)
                .WaitForCompletion();

            _isSmashing = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            /*
            if (!_isColliderActive)
            {
                return;
            }
            */

            if (other.TryGetComponent(out DestructibleObject destructibleObject) && destructibleObject.IsBeingDestroyed)
            {
                return;
            }

            var position = other.gameObject.transform.position;

            Transform newParticleSystem = Instantiate(smashParticleSystem, position, Quaternion.identity);

            bool didHit = Physics.Raycast(
                position, Vector3.down, out RaycastHit hit, 100f, conveyorBeltLayerMask
            );

            if (didHit)
            {
                newParticleSystem.position = hit.point;
            }

            if (destructibleObject != null)
            {
                destructibleObject.DestroySelf(gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}