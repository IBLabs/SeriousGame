using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Common.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent onGameStart;

        [SerializeField] private bool startGameOnStart;
        
        [Header("Dependencies")]
        [SerializeField] private TransitionController transitionController;

        [Header("Configurations")] 
        [SerializeField] private GameOverDescriptor gameOverDescriptor;
        
        private int _amountPassed;
        private int _amountDestroyed;
            
        private void Start()
        {
            if (startGameOnStart) StartGame();
        }

        public void StartGame()
        {
            _amountDestroyed = 0;
            _amountPassed = 0;

            onGameStart.Invoke();
        }

        public void OnGameOver()
        {
            gameOverDescriptor.amountPassed = _amountPassed;
            gameOverDescriptor.amountDetroyed = _amountDestroyed;

            StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            // TODO: show game over screen

            yield return transitionController.StartExitTransitionCoroutine();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnObjectPassed()
        {
            _amountPassed++;
        }
        
        public void OnObjectDestroyed(DestructibleObject destroyedObject)
        {
            if (destroyedObject.Destroyer.GetComponent<ConveyorEndController>() != null) return;

            _amountDestroyed++;
        }
    }
}