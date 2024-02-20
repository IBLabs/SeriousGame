using System;
using DG.Tweening;
using UnityEngine;

namespace Common.Scripts.RevisedLevelsSystem
{
    public class LightsManager : MonoBehaviour
    {
        [SerializeField] private float redRoomDuration = 5.0f;

        private Tweener _ambientLightTweener;
        private float _redRoomEndTime;
        private bool _isRedRoomPlaying;

        private Color _originalAmbientLight;

        private void Update()
        {
            if (_isRedRoomPlaying)
            {
                if (Time.time >= _redRoomEndTime)
                {
                    _isRedRoomPlaying = false;
                    StopRedRoom();
                }
            }
        }

        public void StartRedRoom()
        {
            _redRoomEndTime = Time.time + redRoomDuration;

            if (_isRedRoomPlaying) return;

            _originalAmbientLight = RenderSettings.ambientLight;
            _ambientLightTweener = DOTween
                .To(() => RenderSettings.ambientLight, (x) => RenderSettings.ambientLight = x, Color.red, .66f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            _isRedRoomPlaying = true;
        }

        public void StopRedRoom()
        {
            _ambientLightTweener.Kill();
            DOTween.To(() => RenderSettings.ambientLight, (x) => RenderSettings.ambientLight = x, _originalAmbientLight,
                .2f);
        }
    }
}