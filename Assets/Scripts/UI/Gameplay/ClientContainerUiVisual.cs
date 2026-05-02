using UnityEngine;
using UnityEngine.UI;

namespace Arenar.Services.UI {
    public class ClientContainerUiVisual : MonoBehaviour {
        [SerializeField] protected Image _portrait;
        [SerializeField] protected Image _successCheck;
        [SerializeField] protected Image _failedCheck;

        public void Initialize(Sprite portraitIcon) {
            _portrait.sprite = portraitIcon;
            _failedCheck.gameObject.SetActive(false);
            _successCheck.gameObject.SetActive(false);
        }

        public void SetClientInteractResult(bool isSuccess) {
            _successCheck.gameObject.SetActive(isSuccess);
            _failedCheck.gameObject.SetActive(!isSuccess);
        }
    }
}