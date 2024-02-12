using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent onGameStart;
        public UnityEvent onGameEnd;

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            onGameStart.Invoke();
        }

        public void EndGame()
        {
            onGameEnd.Invoke();
        }
    }
}