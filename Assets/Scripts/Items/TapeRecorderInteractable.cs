using System;
using Arenar.AudioSystem;
using SibGameJam2026;
using SibGameJam2026.Characters;
using SibGameJam2026.Services;
using SibGameJam2026.Settings;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Items {
	/// <summary>
	/// Магнитофон: треки задаются как <see cref="AmbientType"/> (клипы в <see cref="AmbientLibrary"/> у Audio Library);
	/// воспроизведение на собственном <see cref="AudioSource"/> через <see cref="IAudioSystemManager"/> и <see cref="AudioController"/>.
	/// Рабочий режим — пока активный клиент в <see cref="EClientState.WaitCooking"/>.
	/// Пока играет трек — squash-анимация как у <see cref="ItemTransformer"/> / <see cref="ItemMerger"/> (<see cref="ItemProcessingSquashAnimation"/>).
	/// </summary>
	[DisallowMultipleComponent]
	public class TapeRecorderInteractable : MonoBehaviour, IInteractable, ISound {
		[SerializeField] private string _interactItemName = "Магнитофон";
		[SerializeField] private ESoundType _interactionSound = ESoundType.TypeRecorder;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private LocationPlaylist[] _playlists = Array.Empty<LocationPlaylist>();

		[Header("Анимация при воспроизведении")]
		[SerializeField] private Transform _processingVisualTransform;
		[SerializeField] private float _squashHalfCycleSeconds = 0.35f;
		[SerializeField] private float _squashXZStretch = 1.06f;
		[SerializeField] private float _squashYMul = 0.88f;

		[Tooltip("Если для ключа активного уровня нет записи, берётся плейлист с этим ключом; иначе — первый в массиве.")]
		[SerializeField] private string _fallbackLocationKey = "";

		private ILevelService _levelService;
		private IAudioSystemManager _audioSystem;
		private AmbientLibrary _ambientLibrary;
		private AudioController _audioController;
		

		private LocationPlaylist _resolvedPlaylist;
		private bool _workMode;
		private AmbientType _lastPlayedAmbient = AmbientType.None;
		private GameSettingsData _lastActiveLevel;
		private ItemProcessingSquashState _squashState;

		public string InteractItemName => _interactItemName;

		public ESoundType InteractionSound => _interactionSound;

		[Inject]
		private void Construct(ILevelService levelService, IAudioSystemManager audioSystemManager, AudioLibrary audioLibrary) {
			_levelService = levelService;
			_audioSystem = audioSystemManager;
			_ambientLibrary = audioLibrary != null ? audioLibrary.AmbientLibrary : null;
		}

		private void Start() {
			if (_audioSystem == null)
				return;

			_audioSource ??= _audioSystem.CreateAudioSource(gameObject, AudioSystemType.Music);
			_audioController = new AudioController(_audioSource);
		}

		private void Update() {
			if (_levelService == null || _audioController == null || _audioSource == null || _ambientLibrary == null) {
				UpdatePlaybackSquashAnimation(false);
				return;
			}

			var activeLevel = _levelService.ActiveLevel;
			if (activeLevel == null) {
				StopTapePlayback();
				_resolvedPlaylist = null;
				_lastActiveLevel = null;
				UpdatePlaybackSquashAnimation(false);
				return;
			}

			var levelKey = activeLevel.Key;
			var playlist = ResolvePlaylist(levelKey);
			if (playlist == null) {
				StopTapePlayback();
				UpdatePlaybackSquashAnimation(false);
				return;
			}

			var workMode = _levelService.TryGetActiveClientState(out var clientState)
			               && clientState == EClientState.WaitCooking;

			var playlistChanged = !ReferenceEquals(_resolvedPlaylist, playlist);
			var levelChanged = _lastActiveLevel != activeLevel;
			var modeChanged = _workMode != workMode;

			_resolvedPlaylist = playlist;
			_workMode = workMode;
			_lastActiveLevel = activeLevel;

			var trackIds = workMode ? playlist.WorkTracks : playlist.CalmTracks;
			if (trackIds == null || trackIds.Length == 0) {
				StopTapePlayback();
				UpdatePlaybackSquashAnimation(false);
				return;
			}

			var needsNewTrack = playlistChanged || levelChanged || modeChanged || !_audioSource.isPlaying;
			if (needsNewTrack) {
				var next = PickRandomAmbient(trackIds, trackIds.Length > 1 ? _lastPlayedAmbient : AmbientType.None);
				if (next != AmbientType.None)
					PlayAmbientTrack(next);
			}

			UpdatePlaybackSquashAnimation(_audioSource.isPlaying);
		}

		public void OnInteract(InteractContext context) {
			if (_audioController == null || _ambientLibrary == null || _resolvedPlaylist == null)
				return;

			var trackIds = _workMode ? _resolvedPlaylist.WorkTracks : _resolvedPlaylist.CalmTracks;
			if (trackIds == null || trackIds.Length == 0)
				return;

			var next = PickRandomAmbient(trackIds, trackIds.Length > 1 ? _lastPlayedAmbient : AmbientType.None);
			if (next == AmbientType.None)
				return;

			PlayAmbientTrack(next);
		}

		private void OnDestroy() {
			StopTapePlayback();
			ItemProcessingSquashAnimation.Stop(ref _squashState);
		}

		private void OnDisable() {
			StopTapePlayback();
			ItemProcessingSquashAnimation.Stop(ref _squashState);
		}

		private void PlayAmbientTrack(AmbientType type) {
			if (_audioController == null || _ambientLibrary == null || type == AmbientType.None)
				return;

			var clip = _ambientLibrary.GetAmbientByType(type);
			if (clip == null)
				return;

			_audioController.PlaySound(clip, loop: false);
			_lastPlayedAmbient = type;
		}

		private void StopTapePlayback() {
			_audioController?.StopSound();
			_lastPlayedAmbient = AmbientType.None;
		}

		private void UpdatePlaybackSquashAnimation(bool audioPlaying) {
			if (!audioPlaying) {
				if (_squashState.Active)
					ItemProcessingSquashAnimation.Stop(ref _squashState);
				return;
			}

			if (!_squashState.Active) {
				ItemProcessingSquashAnimation.StartOnTransform(
					ref _squashState,
					_processingVisualTransform,
					transform,
					_squashHalfCycleSeconds,
					_squashXZStretch,
					_squashYMul);
			}

			ItemProcessingSquashAnimation.Tick(ref _squashState, Time.deltaTime);
		}

		private LocationPlaylist ResolvePlaylist(string levelKey) {
			if (_playlists == null || _playlists.Length == 0)
				return null;

			foreach (var playlist in _playlists) {
				if (playlist != null && playlist.LocationKey == levelKey)
					return playlist;
			}

			if (string.IsNullOrEmpty(_fallbackLocationKey))
				return _playlists[0];

			foreach (var playlist in _playlists) {
				if (playlist != null && playlist.LocationKey == _fallbackLocationKey)
					return playlist;
			}

			return _playlists[0];
		}

		private static AmbientType PickRandomAmbient(AmbientType[] types, AmbientType avoid) {
			if (types == null || types.Length == 0)
				return AmbientType.None;

			for (var attempt = 0; attempt < 16; attempt++) {
				var t = types[UnityEngine.Random.Range(0, types.Length)];
				if (t == AmbientType.None)
					continue;
				if (types.Length > 1 && t == avoid && attempt < 8)
					continue;
				return t;
			}

			foreach (var type in types) {
				if (type != AmbientType.None)
					return type;
			}

			return AmbientType.None;
		}
	}


	[Serializable]
	public class LocationPlaylist {
		[Tooltip("Должен совпадать с GameSettingsData.Key этого уровня.")]
		public string LocationKey;

		[Tooltip("Ключи в AmbientLibrary (Audio Library). Добавьте значения в enum AmbientType и клипы в словаре.")]
		public AmbientType[] CalmTracks;

		public AmbientType[] WorkTracks;
	}
}
