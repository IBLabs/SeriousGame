using System;
using System.Collections;
using System.Collections.Generic;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public UnityEvent<LevelDescriptor> levelStarted;
        public UnityEvent levelFinished;

        [Header("Configuration")]
        [SerializeField] private float startLevelDelay = 0f;
        [SerializeField] private float levelFinishedEventDelay = 0f;
        [SerializeField] private List<LevelDescriptor> levelDescriptors;
        
        private int _activeObjects = 0;
        public int _currentDescriptorIndex = 0;

        public void StartLevel()
        {
            StartCoroutine(StartLevelCoroutine(startLevelDelay));
        }

        private IEnumerator StartLevelCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            PerformStartLevel();
        }

        private void PerformStartLevel()
        {
            if (levelDescriptors.Count == 0)
            {
                Debug.LogWarning("[WARNING]: no level descriptors configured in the level manager, no level to load");
                return;
            }

            LevelDescriptor currentDescriptor = levelDescriptors[_currentDescriptorIndex];

            _activeObjects = currentDescriptor.amountToSpawn;
            
            levelStarted?.Invoke(currentDescriptor);
        }

        public void HandleObjectSpawned(DestructibleObject destructibleObject)
        {
            if (destructibleObject != null)
            {
                destructibleObject.onObjectDestroyed?.AddListener(HandleObjectDestroyed);
            }
        }

        private void HandleObjectDestroyed(DestructibleObject destructibleObject)
        {
            _activeObjects--;
            if (_activeObjects <= 0)
            {
                StartCoroutine(LevelFinishedCoroutine(levelFinishedEventDelay));
            }
        }

        private IEnumerator LevelFinishedCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            PerformLevelFinished();
        }

        private void PerformLevelFinished()
        {
            if (_currentDescriptorIndex < levelDescriptors.Count - 1)
            {
                _currentDescriptorIndex++;
            }

            levelFinished?.Invoke();
        }
    }
}