using Arenar.AudioSystem;
using Arenar.Options;
using UnityEngine;
using UnityEngine.UI;


namespace Arenar.UI
{
    public abstract class SettingsButton : MonoBehaviour
    {
        [SerializeField] protected Button button = default;

        [Space(10), Header("Icons")]
        [SerializeField] protected Color enableColor = default;
        [SerializeField] protected Color disableColor = default;
        [SerializeField] protected GameObject[] enableObjects = default;
        [SerializeField] protected GameObject[] disableObjects = default;

        protected IOptionsController optionsController;
        protected IUiSoundManager uiSoundManager;


        public virtual void Interactable(bool value) =>
            button.interactable = value;
        
        protected virtual void Initialize()
        {
            button?.onClick.AddListener(OptionsButtonClick);

			SetButtonStatus();
		}

        protected abstract void OptionsButtonClick();

		protected abstract void SetButtonStatus();

        protected void SetIcons(bool isActive)
        {
            SetEnableObjectsActive(isActive);
            SetDisableObjectsActive(!isActive);

            button.image.color = (isActive) ? enableColor : disableColor;
        }

        private void SetEnableObjectsActive(bool isActive)
        {
            foreach (var enableObject in enableObjects)
                enableObject.SetActive(isActive);
        }

        private void SetDisableObjectsActive(bool isActive)
        {
            foreach (var disableObject in disableObjects)
                disableObject.SetActive(isActive);
        }

		private void OnEnable() =>
			SetButtonStatus();
	}
}
