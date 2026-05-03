using Arenar.AudioSystem;
using Arenar.Options;
using UnityEngine;
using Zenject;


namespace Arenar.UI
{
    public class AudioSettingsButton : SettingsButton
    {
        [SerializeField] protected AudioSystemType[] audioTypes = default;

        private IAudioSystemManager audioSystem;


        [Inject]
        private void Initialize(IAudioSystemManager audioSystem,
            IUiSoundManager uiSoundManager,
            IOptionsController optionsController)
        {
            this.audioSystem = audioSystem;
            this.uiSoundManager = uiSoundManager;
            this.optionsController = optionsController;

            Initialize();
        }

		protected override void OptionsButtonClick()
        {
            bool isActive = true;

            foreach (AudioSystemType type in audioTypes)
            {
                switch (type)
                {
                    case AudioSystemType.Music:
                        isActive = GetCoupedAudioOptionValue<MusicOption>(type);
                        break;

                    case AudioSystemType.Sound:
                        isActive = GetCoupedAudioOptionValue<SoundOption>(type);
                        break;

                    case AudioSystemType.UI:
                        isActive = GetCoupedAudioOptionValue<UiSoundOption>(type);
                        break;

                    default:
                        Debug.LogError($"Unknown audio type {type}");
                        continue;
                }

                SetIcons(isActive);
            }

            uiSoundManager.PlaySound(UiSoundType.StandartButtonClick);
        }

		protected override void SetButtonStatus()
		{
			foreach (AudioSystemType type in audioTypes)
			{
				bool isActive = true;

				switch (type)
				{
					case AudioSystemType.Music:
						isActive = optionsController.GetOption<MusicOption>().IsActive;
						break;

					case AudioSystemType.Sound:
						isActive = optionsController.GetOption<SoundOption>().IsActive;
						break;

					case AudioSystemType.UI:
						isActive = optionsController.GetOption<UiSoundOption>().IsActive;
						break;

					default:
						Debug.LogError($"Unknown audio type {type}");
						continue;
				}

				SetIcons(isActive);
			}
		}

		private void SetAudioOption<T>(AudioSystemType type, bool isActive, float volume) where T : IOption
        {
            T audioOption = optionsController.GetOption<T>();

            switch (audioOption)
            {
                case MusicOption musicOption:
                    musicOption.IsActive = isActive;
                    musicOption.Volume = volume;
                    optionsController.SetOption(musicOption);
                    break;

                case SoundOption soundOption:
                    soundOption.IsActive = isActive;
                    soundOption.Volume = volume;
                    optionsController.SetOption(soundOption);
                    break;

                case UiSoundOption uiSoundOption:
                    uiSoundOption.IsActive = isActive;
                    uiSoundOption.Volume = volume;
                    optionsController.SetOption(uiSoundOption);
                    break;

                default:
                    Debug.LogError($"Unknown IOption type {typeof(T)} for audio button.");
                    return;
            }

            audioSystem.SetVolume(type, isActive, volume);
        }

        private bool GetCoupedAudioOptionValue<T>(AudioSystemType type) where T : IOption
        {
            T audioOption = optionsController.GetOption<T>();

            switch (audioOption)
            {
                case MusicOption musicOption:
                    musicOption.IsActive = !musicOption.IsActive;
                    optionsController.SetOption(musicOption);
                    audioSystem.SetVolume(type, musicOption.IsActive, musicOption.Volume);
                    return musicOption.IsActive;

                case SoundOption soundOption:
                    soundOption.IsActive = !soundOption.IsActive;
                    optionsController.SetOption(soundOption);
                    audioSystem.SetVolume(type, soundOption.IsActive, soundOption.Volume);
                    return soundOption.IsActive;

                case UiSoundOption uiSoundOption:
                    uiSoundOption.IsActive = !uiSoundOption.IsActive;
                    optionsController.SetOption(uiSoundOption);
                    audioSystem.SetVolume(type, uiSoundOption.IsActive, uiSoundOption.Volume);
                    return uiSoundOption.IsActive;

                default:
                    Debug.LogError($"Unknown IOption type {typeof(T)} for audio button.");
                    return true;
            }
        }
    }
}
