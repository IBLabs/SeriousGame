using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PattyShaker : MonoBehaviour
{
    void Start()
    {
        transform
            .DOMoveY(transform.position.y + 0.5f, 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        transform
            .DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 5), 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .From(transform.rotation.eulerAngles + new Vector3(0, 0, -5));
    }
}
