using System.Collections.Generic;
using Arenar.AudioSystem;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Services {
	public interface IInteractionSoundService {
		/// <param name="soundHost">Объект взаимодействия (чайник и т.п.); на нём создаётся <see cref="AudioSource"/>.</param>
		void Play(GameObject soundHost, ESoundType type);
	}

	public class InteractionSoundService : IInteractionSoundService {
		private readonly AudioLibrary _audioLibrary;
		private readonly IAudioSystemManager _audioSystem;
		private readonly ISoundManager _soundManager;

		private readonly Dictionary<int, AudioSource> _sourceByHostId = new();

		[Inject]
		public InteractionSoundService(
			AudioLibrary audioLibrary,
			IAudioSystemManager audioSystem,
			ISoundManager soundManager
		) {
			_audioLibrary = audioLibrary;
			_audioSystem = audioSystem;
			_soundManager = soundManager;
		}

		public void Play(GameObject soundHost, ESoundType type) {
			if (soundHost == null || type == ESoundType.None || _audioLibrary?.SoundsLibrary == null)
				return;

			var clip = _audioLibrary.SoundsLibrary.GetInteractionClip(type);
			if (clip == null) {
				Debug.LogWarning(
					$"[{nameof(InteractionSoundService)}] Нет клипа в Sounds Library → Interaction для {type}.");
				return;
			}

			var id = soundHost.GetInstanceID();
			if (!_sourceByHostId.TryGetValue(id, out var source) || source == null) {
				source = _audioSystem.CreateAudioSource(soundHost, AudioSystemType.Sound);
				_sourceByHostId[id] = source;
			}

			_soundManager.PlaySound(source, clip, false);
		}
	}
}
