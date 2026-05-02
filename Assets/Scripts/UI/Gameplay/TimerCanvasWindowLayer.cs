using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arenar.Services.UI {
    public class TimerCanvasWindowLayer : CanvasWindowLayer {
        [SerializeField] private Image _timerImage;


        public void SetTimerProgress(float progress, float progressMax) {
            _timerImage.fillAmount = progress / progressMax;
        }
    }
}