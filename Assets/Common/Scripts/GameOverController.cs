using System;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip printerAudioClip;
        [SerializeField] private AudioClip stampAudioClip;
        [SerializeField] private RectTransform cardContainer;
        [SerializeField] private Image cardPrinterShadow;
        [SerializeField] private TextMeshProUGUI kgText;
        [SerializeField] private TextMeshProUGUI funFactText;
        
        [SerializeField] private AudioSource bgmAudioSource;

        private void Start()
        {
            SetInitialState();
            
            bgmAudioSource.DOFade(1f, 1f).From(0f);
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
            
            float cardContainerHeight = cardContainer.rect.height * cardContainer.localScale.y;
            cardContainer.anchoredPosition = new Vector2(cardContainer.anchoredPosition.x, -cardContainerHeight);
            
            cardPrinterShadow.color = new Color(cardPrinterShadow.color.r, cardPrinterShadow.color.g, cardPrinterShadow.color.b, 0);
        }

        private IEnumerator RestartGameCoroutine()
        {
            yield return transitionController.StartExitTransitionCoroutine();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void ConfigureUI()
        {
            // StartCoroutine(ConfigureEnvelopeUICoroutine());

            StartCoroutine(ConfigureCardUICoroutine());
        }

        private IEnumerator ConfigureCardUICoroutine()
        {
            kgText.alpha = 1;
            funFactText.alpha = 1;
            
            float cardContainerHeight = cardContainer.rect.height * cardContainer.localScale.y;
            cardContainer.anchoredPosition = new Vector2(cardContainer.anchoredPosition.x, -cardContainerHeight);

            kgText.text = $"{gameOverDescriptor.amountDetroyed.ToString()}kg";

            List<string> funFacts = new();
            funFacts.Add(FoodWasteImpactCalculator.GetCO2FunFactString(gameOverDescriptor.amountDetroyed));
            funFacts.Add(FoodWasteImpactCalculator.GetWaterWasteFunFactString(gameOverDescriptor.amountDetroyed));
            funFacts.Add(FoodWasteImpactCalculator.GetLandfillContributionFunFactString(gameOverDescriptor.amountDetroyed));
            funFactText.text = funFacts[UnityEngine.Random.Range(0, funFacts.Count)];
            
            audioSource.PlayOneShot(printerAudioClip);
            cardPrinterShadow.DOFade(1f, 0.2f).SetEase(Ease.Linear);
            yield return cardContainer.DOAnchorPosY(0, 4.3f).SetEase(Ease.Linear).WaitForCompletion();

            yield return new WaitForSeconds(2f);
            
            yield return playAgainButtonCanvasGroup.DOFade(1f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();
        }
        
        

        private IEnumerator ConfigureEnvelopeUICoroutine()
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