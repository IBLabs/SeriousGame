using System;
using DG.Tweening;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    public class LightsManager : MonoBehaviour
    {
        [SerializeField] private float redRoomDuration = 5.0f;

        [ColorUsage(true, true)]
        [SerializeField] private Color initialAmbientLightColor;

        private Tweener _ambientLightTweener;
        private float _redRoomEndTime;
        private bool _isRedRoomPlaying;

        private void OnDestroy()
        {
            StopRedRoom(false);
        }

        private void Update()
        {
            if (_isRedRoomPlaying)
            {
                if (Time.time >= _redRoomEndTime)
                {
                    _isRedRoomPlaying = false;
                    StopRedRoom(true);
                }
            }
        }

        public void StartRedRoom()
        {
            _redRoomEndTime = Time.time + redRoomDuration;

            if (_isRedRoomPlaying) return;

            _ambientLightTweener = DOTween
                .To(() => RenderSettings.ambientLight, (x) => RenderSettings.ambientLight = x, Color.red, .66f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            _isRedRoomPlaying = true;
        }

        public void StopRedRoom(bool animated)
        {
            if (_ambientLightTweener != null)
            {
                _ambientLightTweener.Kill();
                _ambientLightTweener = null;
            }

            if (animated)
            {
                DOTween.To(() => RenderSettings.ambientLight, (x) => RenderSettings.ambientLight = x, initialAmbientLightColor,
                    .2f);
            }
            else
            {
                RenderSettings.ambientLight = initialAmbientLightColor;
            
            }
        }
    }
}