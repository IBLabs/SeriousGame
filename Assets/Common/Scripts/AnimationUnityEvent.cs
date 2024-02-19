using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class AnimationUnityEventItem
{
    public string animationName;
    public UnityEvent animationEvents;    
}

public class AnimationUnityEvent : MonoBehaviour
{
    public AnimationUnityEventItem[] items;
    
    public void OnAnimationEvent(AnimationEvent animationEvent)
    {
        foreach (var item in items)
        {
            if (item.animationName == animationEvent.animatorClipInfo.clip.name)
            {
                item.animationEvents.Invoke();
            }
        }
    }
}
