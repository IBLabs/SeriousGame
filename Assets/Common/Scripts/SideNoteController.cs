using System;
using Common.Scripts.RevisedLevelsSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Scripts
{
    public class SideNoteController : MonoBehaviour
    {
        [SerializeField] private Image spotsImage;
        [SerializeField] private Image bigImage;
        [SerializeField] private Image smallImage;

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
                GetComponent<RectTransform>().DOAnchorPosX(-_slideOffset, .4f).SetEase(Ease.OutCubic).SetRelative(true);
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition -= new Vector2(-_slideOffset, 0);
            }
        }

        public void Hide(bool animated)
        {
            if (!_isVisible) return;
            
            _isVisible = false;

            if (animated)
            {
                GetComponent<RectTransform>().DOAnchorPosX(_slideOffset, .4f).SetEase(Ease.InCubic).SetRelative(true);    
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition += new Vector2(_slideOffset, 0);
            }
        }

        public void OnLevelStart(LevelDescriptor levelDescriptor)
        {
            bool isSmallAllowed = levelDescriptor.ruleSet.allowedSizes.Contains(ConveyorObjectSizeClass.SMALL);
            bool isBigAllowed = levelDescriptor.ruleSet.allowedSizes.Contains(ConveyorObjectSizeClass.BIG);
            bool isSpotsAllowed = levelDescriptor.ruleSet.spotsAllowed;
            
            spotsImage.DOFade((!isSpotsAllowed) ? 1 : .2f, .2f);
            bigImage.DOFade((!isBigAllowed) ? 1 : .2f, .2f);
            smallImage.DOFade((!isSmallAllowed) ? 1 : .2f, .2f);
        }
    }
}