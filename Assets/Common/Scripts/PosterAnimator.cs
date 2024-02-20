using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PosterAnimator : MonoBehaviour
{
    [SerializeField] private float shakeStrength = .2f;
    [SerializeField] private float shakeDuration = 2;

    void Start()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(0);
            DOTween.To(() => skinnedMeshRenderer.GetBlendShapeWeight(0), x => skinnedMeshRenderer.SetBlendShapeWeight(0, x), blendShapeWeight + shakeStrength, shakeDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }
}
