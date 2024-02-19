using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public UnityEvent levelFinished;
        
        [SerializeField] private ObjectSpawner objectSpawner;

        [SerializeField] private float startLevelDelay = 0f;
        [SerializeField] private float levelFinishedEventDelay = 0f;

        private int _levelTargetActiveObjects = 2;
        private int _levelAmountSpawned;

        private int _activeObjects = 0;

        private void Start()
        {
            objectSpawner.onObjectSpawned.AddListener(HandleObjectSpawned);
        }

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
            objectSpawner.StartSpawning();

            _activeObjects = _levelTargetActiveObjects;
        }

        private void HandleObjectSpawned(DestructibleObject destructibleObject)
        {
            _levelAmountSpawned++;

            if (destructibleObject != null)
            {
                destructibleObject.onObjectDestroyed?.AddListener(HandleObjectDestroyed);
            }

            if (_levelAmountSpawned >= _levelTargetActiveObjects)
            {
                _levelAmountSpawned = 0;
                _levelTargetActiveObjects += 3;
                objectSpawner.StopSpawning();
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
            levelFinished?.Invoke();
        }
    }
}