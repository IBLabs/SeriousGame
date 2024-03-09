using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Common.Scripts
{
    public class HealthController : MonoBehaviour
    {
        public UnityEvent onPlayerDeath;
        
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private HorizontalLayoutGroup heartsContainer;

        [SerializeField] private int initialHeartCount = 3;

        private int _heartCount;
        private List<GameObject> _hearts = new();

        private void Start()
        {
            _heartCount = initialHeartCount;
            
            for (var i = 0; i < _heartCount; i++)
            {
                GameObject newHeart = Instantiate(heartPrefab, heartsContainer.transform);
                _hearts.Add(newHeart);
            }
        }

        public void OnWrongObjectPassed()
        {
            DecreaseHealth();   
        }
        
        public void OnWrongObjectDestroyed()
        {
            DecreaseHealth();
        }

        private void DecreaseHealth()
        {
            if (_heartCount > 0)
            {
                _heartCount--;

                StartCoroutine(DestroyHeartCoroutine(_hearts[_heartCount]));
            }
            
            if (_heartCount == 0)
            {
                onPlayerDeath?.Invoke();
            }
        }

        private IEnumerator DestroyHeartCoroutine(GameObject heartGameObject)
        {
            if (heartGameObject.TryGetComponent(out Image heartImage))
            {
                yield return heartImage.DOFade(0f, 1f).SetEase(Ease.Flash, 3, 0).WaitForCompletion();
            }
            
            Destroy(heartGameObject);
            _hearts.RemoveAt(_hearts.Count - 1);
        }
    }
}