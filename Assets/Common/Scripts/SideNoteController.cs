using System;
using System.Collections;
using Common.Scripts.RevisedLevelsSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.Scripts
{
    public class SideNoteController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip enterAudioClip;
        [SerializeField] private AudioClip exitAudioClip;
        [SerializeField] private AudioClip stampAudioClip;

        [SerializeField] private Image spotsImage;
        [SerializeField] private Image bigImage;
        [SerializeField] private Image smallImage;

        public UnityEvent onFinishEnterAnimation;
        public UnityEvent onFinishExitAnimation;
        public UnityEvent onRuleSetUpdateFinished;

        private bool _isVisible = true;
        private float _slideOffset = 300f;

        private void Start()
        {
            Hide(false);
        }

        public void Show(bool animated)
        {
            if (_isVisible) return;

            _isVisible = true;

            if (animated)
            {
                StartCoroutine(ShowCoroutine());
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition += new Vector2(0, _slideOffset);
            }
        }

        private IEnumerator ShowCoroutine()
        {
            audioSource.PlayOneShot(enterAudioClip);

            yield return GetComponent<RectTransform>().DOAnchorPosY(_slideOffset, .4f).SetEase(Ease.OutCubic).SetRelative(true).WaitForCompletion();

            onFinishEnterAnimation?.Invoke();
        }

        public void Hide(bool animated)
        {
            if (!_isVisible) return;
            
            _isVisible = false;

            if (animated)
            {
                StartCoroutine(HideCoroutine());
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, _slideOffset);
            }
        }

        private IEnumerator HideCoroutine()
        {
            audioSource.PlayOneShot(exitAudioClip);

            yield return GetComponent<RectTransform>().DOAnchorPosY(-_slideOffset, .4f).SetEase(Ease.InCubic).SetRelative(true).WaitForCompletion();
            
            spotsImage.DOFade(0.2f, .1f);
            bigImage.DOFade(0.2f, .1f);
            smallImage.DOFade(0.2f, .1f);
            
            onFinishExitAnimation?.Invoke();
        }

        public void OnLevelStart(LevelDescriptor levelDescriptor)
        {
            StartCoroutine(UpdateRuleSetCoroutine(levelDescriptor));
        }

        private IEnumerator UpdateRuleSetCoroutine(LevelDescriptor levelDescriptor)
        {
            bool isSmallAllowed = levelDescriptor.ruleSet.allowedSizes.Contains(ConveyorObjectSizeClass.SMALL);
            bool isBigAllowed = levelDescriptor.ruleSet.allowedSizes.Contains(ConveyorObjectSizeClass.BIG);
            bool isSpotsAllowed = levelDescriptor.ruleSet.spotsAllowed;

            if (!isSpotsAllowed)
            {
                yield return new WaitForSeconds(.7f);

                audioSource.PlayOneShot(stampAudioClip);
                yield return spotsImage.DOFade(1, .1f).WaitForCompletion();
            }

            if (!isBigAllowed)
            {
                yield return new WaitForSeconds(.7f);

                audioSource.PlayOneShot(stampAudioClip);
                yield return bigImage.DOFade(1, .1f).WaitForCompletion();
            }

            if (!isSmallAllowed)
            {
                yield return new WaitForSeconds(.7f);
            
                audioSource.PlayOneShot(stampAudioClip);
                yield return smallImage.DOFade(1, .1f).WaitForCompletion();
            }

            yield return new WaitForSeconds(1f);
            
            onRuleSetUpdateFinished?.Invoke();
        }
    }
}