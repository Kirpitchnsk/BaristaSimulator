using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arenar.Services.UI {
    public class DialogAnswerUiVisual : MonoBehaviour {
        [SerializeField] private TMP_Text _answerText;
        [SerializeField] private Button _acceptButton;

        public void SetAnswer(string answerText, Action callback) {
            _answerText.text = answerText;
            
            _acceptButton?.onClick.RemoveAllListeners();
            _acceptButton?.onClick.AddListener(() => callback?.Invoke());
        }
    }
}