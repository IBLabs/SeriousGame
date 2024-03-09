using System;
using System.Collections;
using Common.Scripts.RevisedLevelsSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common.Scripts
{
    public class GameOverController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TransitionController transitionController;
        [SerializeField] private LightsManager lightsManager;
        
        [Header("Configurations")] 
        [SerializeField] private GameOverDescriptor gameOverDescriptor;

        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private TextMeshProUGUI passedText;
        [SerializeField] private TextMeshProUGUI destroyedText;
        [SerializeField] private GridLayoutGroup personContainer;
        [SerializeField] private CanvasGroup playAgainButtonCanvasGroup;

        [SerializeField] private Image personImagePrefab;

        private void Start()
        {
            SetInitialState();
        }

        public void RestartGame()
        {
            StartCoroutine(RestartGameCoroutine());
        }

        private void SetInitialState()
        {
            lightsManager.StopRedRoom(false);

            passedText.text = "000";
            destroyedText.text = "000";
            playAgainButtonCanvasGroup.alpha = 0;
            
            string dateString = DateTime.Now.ToString("dddd, d MMMM yyyy");
            subtitleText.text = $"Date of Issue: {dateString}";
        }

        private IEnumerator RestartGameCoroutine()
        {
            yield return transitionController.StartExitTransitionCoroutine();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void ConfigureUI()
        {
            StartCoroutine(ConfigureUICoroutine());
        }

        private IEnumerator ConfigureUICoroutine()
        {
            yield return new WaitForSeconds(0.5f);

            if (gameOverDescriptor.amountPassed > 0)
            {
                yield return DOTween.To(() => int.Parse(passedText.text), x => passedText.text = x.ToString("D3"), gameOverDescriptor.amountPassed, 1.0f).WaitForCompletion();
                yield return new WaitForSeconds(0.5f);
            }

            if (gameOverDescriptor.amountDetroyed > 0)
            {
                yield return DOTween.To(() => int.Parse(destroyedText.text), x => destroyedText.text = x.ToString("D3"), gameOverDescriptor.amountDetroyed, 1.0f).WaitForCompletion();
                yield return new WaitForSeconds(0.5f);    
            }
            
            int personUnfed = CalculatePersonUnfed(gameOverDescriptor.amountDetroyed);

            if (personUnfed > 0)
            {
                int personFed = CalculatePersonFed(gameOverDescriptor.amountPassed);

                for (var i = 0; i < personUnfed; i++)
                {
                    Image newPerson = Instantiate(personImagePrefab, personContainer.transform);
                    newPerson.color = (i < personFed) ? Color.green : Color.red;

                    yield return new WaitForSeconds(0.2f);
                }
            }

            playAgainButtonCanvasGroup.DOFade(1f, 0.3f).SetEase(Ease.Linear);
        }

        private int CalculatePersonFed(int amountPassed)
        {
            return (int)(amountPassed * 0.5f);
        }

        private int CalculatePersonUnfed(int amountDestroyed)
        {
            return (int)(amountDestroyed * 1.5f);
        }
    }
}