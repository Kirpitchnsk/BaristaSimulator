using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arenar.Services.UI {
    public class TimerCanvasWindowLayer : CanvasWindowLayer {
        [SerializeField] private Image _timerImage;
        [SerializeField] private Color _greenColor = Color.green;
        [SerializeField] private Color _yellowColor = Color.yellow;
        [SerializeField] private Color _redColor = Color.red;

        public void SetLayerEnabled(bool isEnabled) {
            gameObject.SetActive(isEnabled);
        }

        public void SetTimerProgress(float progress, float progressMax) {
            if (_timerImage == null)
                return;

            if (progressMax <= 0f) {
                _timerImage.fillAmount = 0f;
                _timerImage.color = _greenColor;
                return;
            }

            var normalized = Mathf.Clamp01(progress / progressMax);
            _timerImage.fillAmount = normalized;

            if (normalized <= 0.1f)
                _timerImage.color = _redColor;
            else if (normalized <= 0.5f)
                _timerImage.color = _yellowColor;
            else
                _timerImage.color = _greenColor;
        }
    }
}