using DG.Tweening;
using UnityEngine;

namespace Common.Scripts
{
    public class FeedbackPlateController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer plateMeshRenderer;
        [SerializeField] private Transform plateTransform;

        [ColorUsage(true, true)]
        [SerializeField] private Color correctColor;
        
        [ColorUsage(true, true)]
        [SerializeField] private Color wrongColor;

        private Tweener _activeTween;

        private bool _isActiveStateCorrect = true;

        private void CancelActiveTweenIfAny()
        {
            if (_activeTween != null)
            {
                _activeTween.Kill();
                _activeTween = null;
            }
        }
        
        public void SetFeedbackGood()
        {
            CancelActiveTweenIfAny();
            plateMeshRenderer.materials[0].SetColor("_EmissionColor", correctColor);
            _activeTween = plateMeshRenderer.materials[0].DOColor(Color.black, "_EmissionColor", 3f).SetEase(Ease.Flash, 3, 0);
            
            plateTransform.DORotate(new Vector3(360f, 0, 0f), 1f, RotateMode.FastBeyond360).From(new Vector3(_isActiveStateCorrect ? 0f : 180f, 0, 0)).SetEase(Ease.OutBack);
            _isActiveStateCorrect = true;
        }
        
        public void SetFeedbackBad()
        {
            CancelActiveTweenIfAny();
            plateMeshRenderer.materials[0].SetColor("_EmissionColor", wrongColor);
            _activeTween = plateMeshRenderer.materials[0].DOColor(Color.black, "_EmissionColor", 3f).SetEase(Ease.Flash, 3, 0);
            
            plateTransform.DORotate(new Vector3(540f, 0, 0f), 1f, RotateMode.FastBeyond360).From(new Vector3(_isActiveStateCorrect ? 360f : 180f, 0 ,0)).SetEase(Ease.OutBack);
            _isActiveStateCorrect = false;
        }
    }
}