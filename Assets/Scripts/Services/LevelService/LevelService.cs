using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arenar.Services.UI;
using SibGameJam2026.Cameras;
using SibGameJam2026.Characters;
using SibGameJam2026.Characters.Components;
using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using SibGameJam2026.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace SibGameJam2026.Services {
	public class LevelService : ILevelService, ITickable {
		private const string ClientsExhaustedLog = "[LevelService] Персонажи закончились";

		private readonly ACharacter.Factory _characterFactory;
		private readonly ICameraService _cameraService;
		private readonly GameSettingsData[] _gameSettingsData;
		private readonly ItemsDatabase _itemsDatabase;
		private readonly IMergeSystem _mergeSystem;
		private readonly CarManager _carManager;

		private readonly LazyInject<GameplayCanvasWindowController> _gameplayCanvasController;

		private readonly Dictionary<int, ItemId> _clientExpectedDishByInstanceId = new();

		private GameSettingsData _activeLevel;
		private INpcControlStateCharacterComponent _activeClientNpcState;
		private int _nextClientIndex;
		private int _currentActiveClientIndex = -1;
		private int _cookSuccessCount;
		private int _cookFailureCount;
		private float _activeCookDeadline;
		private float _activeCookDuration;
		private bool _isCookTimerRunning;
		private GameObject _activeLevelLocationInstance;
		private AsyncOperationHandle<GameObject> _activeLevelLocationHandle;
		private bool _hasActiveLevelLocationHandle;
		private int _locationLoadVersion;

		public GameSettingsData ActiveLevel => _activeLevel;
		public int CookFailureCount => _cookFailureCount;
		public int CookSuccessCount => _cookSuccessCount;
		public int CurrentActiveClientIndex => _currentActiveClientIndex;

		public LevelService(
			ACharacter.Factory characterFactory,
			ICameraService cameraService,
			GameSettingsData[] gameSettingsData,
			[InjectOptional] ItemsDatabase itemsDatabase,
			[InjectOptional] IMergeSystem mergeSystem,
			[InjectOptional] CarManager carManager,
			LazyInject<GameplayCanvasWindowController> gameplayCanvasController
		) {
			_characterFactory = characterFactory;
			_cameraService = cameraService;
			_gameSettingsData = gameSettingsData;
			_itemsDatabase = itemsDatabase;
			_mergeSystem = mergeSystem;
			_carManager = carManager;
			_gameplayCanvasController = gameplayCanvasController;
		}

		public void BeginLevel(string levelKey = null) {
			_activeLevel = ResolveLevel(levelKey);
			UnloadLevelLocation();

			_nextClientIndex = 0;
			_currentActiveClientIndex = -1;
			_activeClientNpcState = null;
			_cookSuccessCount = 0;
			_cookFailureCount = 0;
			_activeCookDuration = 0f;
			_isCookTimerRunning = false;

			_clientExpectedDishByInstanceId.Clear();

			if (_activeLevel == null)
				Debug.LogWarning($"[{nameof(LevelService)}] Уровень не найден (ключ '{levelKey}'). Очередь NPC не запущена.");
			else {
				_locationLoadVersion++;
				_ = LoadLevelLocationAsync(_activeLevel, _locationLoadVersion);
				ResetGameplayUi(_activeLevel);
			}

			var player = _characterFactory.Create(ECharacterType.Player, Vector3.zero);
			var cameraParent = player.Data.CameraPoint != null ? player.Data.CameraPoint : player.transform;
			_cameraService.AttachActiveCameraTo(cameraParent);

			TrySpawnNextClientNpc();
		}

		public void EndLevel() {
			_isCookTimerRunning = false;
			_activeCookDuration = 0f;
			_activeClientNpcState = null;
			_currentActiveClientIndex = -1;
			_clientExpectedDishByInstanceId.Clear();
			_gameplayCanvasController.Value?.SetCookTimerActive(false);
			UnloadLevelLocation();
		}

		public void AssignCookingDishForClient(ACharacter clientNpc) {
			var key = clientNpc.gameObject.GetInstanceID();
			if (_clientExpectedDishByInstanceId.ContainsKey(key))
				return;

			var dish = PickDishForNewOrder();
			_clientExpectedDishByInstanceId[key] = dish;
			Debug.Log($"[{nameof(LevelService)}] Dish assigned for cooking: {dish} (client {clientNpc.name}).");
		}

		public bool TryGetExpectedDish(ACharacter clientNpc, out ItemId dishId) {
			return _clientExpectedDishByInstanceId.TryGetValue(clientNpc.gameObject.GetInstanceID(), out dishId);
		}

		public void Tick() {
			_gameplayCanvasController.Value?.UpdateFocusItemMark();
			_gameplayCanvasController.Value?.SetCookTimerActive(_isCookTimerRunning);
			if (!_isCookTimerRunning)
				return;

			var remaining = Mathf.Max(0f, _activeCookDeadline - Time.time);
			_gameplayCanvasController.Value?.UpdateCookTimer(remaining, _activeCookDuration);
			if (remaining > 0f)
				return;

			_isCookTimerRunning = false;
			_gameplayCanvasController.Value?.SetCookTimerActive(false);
			SetCookFailed();
			if (_activeClientNpcState != null && _activeClientNpcState.State == EClientState.WaitCooking)
				_activeClientNpcState.SetState(EClientState.NonTransformed);
		}

		public void SetCookFailed() {
			if (_currentActiveClientIndex < 0)
				return;

			var list = _activeLevel?.ClientData;
			if (list == null || _currentActiveClientIndex >= list.Count)
				return;

			_cookFailureCount++;
			_gameplayCanvasController.Value?.SetClientCookFailed(_currentActiveClientIndex);
		}

		public void SetCookSuccess() {
			if (_currentActiveClientIndex < 0)
				return;

			var list = _activeLevel?.ClientData;
			if (list == null || _currentActiveClientIndex >= list.Count)
				return;

			_cookSuccessCount++;
			_gameplayCanvasController.Value?.SetClientCookSuccess(_currentActiveClientIndex);
		}

		public void StartActiveClientCookingTimer() {
			if (_activeLevel == null || _currentActiveClientIndex < 0) {
				_isCookTimerRunning = false;
				_activeCookDuration = 0f;
				_gameplayCanvasController.Value?.SetCookTimerActive(false);
				return;
			}

			var clients = _activeLevel.ClientData;
			if (clients == null || _currentActiveClientIndex >= clients.Count) {
				_isCookTimerRunning = false;
				_activeCookDuration = 0f;
				_gameplayCanvasController.Value?.SetCookTimerActive(false);
				return;
			}

			var timeoutSeconds = clients[_currentActiveClientIndex].CookTimeoutSeconds;
			if (timeoutSeconds <= 0f) {
				_isCookTimerRunning = false;
				_activeCookDuration = 0f;
				_gameplayCanvasController.Value?.SetCookTimerActive(false);
				return;
			}

			_activeCookDuration = timeoutSeconds;
			_activeCookDeadline = Time.time + timeoutSeconds;
			_isCookTimerRunning = true;
			_gameplayCanvasController.Value?.SetCookTimerActive(true);
			_gameplayCanvasController.Value?.UpdateCookTimer(_activeCookDuration, _activeCookDuration);
		}

		public bool TryGetActiveClientState(out EClientState state) {
			if (_activeClientNpcState != null) {
				state = _activeClientNpcState.State;
				return true;
			}

			state = default;
			return false;
		}

		public void PresentClientOrderUi(ACharacter clientNpc) {
			if (!TryGetExpectedDish(clientNpc, out var dishId))
				return;

			IReadOnlyList<ItemId> ingredientIds = Array.Empty<ItemId>();
			if (_mergeSystem != null && _mergeSystem.TryGetSourceProductIds(dishId, out var fromRecipe))
				ingredientIds = fromRecipe;

			_gameplayCanvasController.Value?.ShowCookMenu(dishId, ingredientIds);
		}

		private GameSettingsData ResolveLevel(string levelKey) {
			if (_gameSettingsData == null || _gameSettingsData.Length == 0)
				return null;

			if (string.IsNullOrEmpty(levelKey))
				return _gameSettingsData[0];
			
			foreach (var s in _gameSettingsData) {
				if (s != null && s.Key == levelKey)
					return s;
			}

			Debug.LogWarning($"[{nameof(LevelService)}] Ключ уровня '{levelKey}' не найден среди {nameof(GameSettingsData)}.");

			return _gameSettingsData[0];
		}

		private void TrySpawnNextClientNpc() {
			if (_activeLevel == null)
				return;

			var list = _activeLevel.ClientData;
			if (list == null || _nextClientIndex >= list.Count) {
				Debug.Log(ClientsExhaustedLog);
				return;
			}

			var data = list[_nextClientIndex];
			var spawn = _carManager != null && _carManager.ClientSpawnPoint != null
				? _carManager.ClientSpawnPoint.position
				: Vector3.zero;

			var character = _characterFactory.Create(data.ECharacterType, spawn);
			var instanceId = character.gameObject.GetInstanceID();
			_clientExpectedDishByInstanceId[instanceId] = data.ItemId;
			_currentActiveClientIndex = _nextClientIndex;
			_nextClientIndex++;

			if (character.TryGetComponent<INpcControlStateCharacterComponent>(out var npcState)) {
				_activeClientNpcState = npcState;
				void Handler(EClientState state) {
					if (state != EClientState.WaitCooking) {
						_isCookTimerRunning = false;
						_activeCookDuration = 0f;
						_gameplayCanvasController.Value?.SetCookTimerActive(false);
					}
					if (state != EClientState.Finished)
						return;
					npcState.StateChanged -= Handler;
					if (_activeClientNpcState == npcState)
						_activeClientNpcState = null;
					TrySpawnNextClientNpc();
				}

				npcState.StateChanged += Handler;
			} else {
				_activeClientNpcState = null;
				Debug.LogWarning(
					$"[{nameof(LevelService)}] У {character.name} нет {nameof(INpcControlStateCharacterComponent)} — очередь клиентов не продолжится, пока не будет FSM.");
			}
		}

		private ItemId PickDishForNewOrder() {
			if (_itemsDatabase != null && _itemsDatabase.Items.Count > 0) {
				for (var i = 0; i < _itemsDatabase.Items.Count; i++) {
					var item = _itemsDatabase.Items[i];
					if (item.ItemType == EItemType.Food) {
						return item.ItemId;
					}
				}

				return _itemsDatabase.Items[0].ItemId;
			}

			return new ItemId("placeholder_dish");
		}

		private void ResetGameplayUi(GameSettingsData gameData) {
			_gameplayCanvasController.Value?.SetupForLevel(gameData);
		}

		private async Task LoadLevelLocationAsync(GameSettingsData levelData, int loadVersion) {
			if (levelData == null || levelData.Location == null || !levelData.Location.RuntimeKeyIsValid())
				return;

			try {
				var handle = levelData.Location.InstantiateAsync(Vector3.zero, Quaternion.identity);
				var instance = await handle.Task;

				// Если в процессе загрузки уровень сменился, выгружаем устаревший инстанс.
				if (loadVersion != _locationLoadVersion) {
					Addressables.ReleaseInstance(handle);
					return;
				}

				_activeLevelLocationHandle = handle;
				_hasActiveLevelLocationHandle = true;
				_activeLevelLocationInstance = instance;

				if (_activeLevelLocationInstance != null)
					_activeLevelLocationInstance.transform.position = Vector3.zero;
			} catch (Exception e) {
				Debug.LogError($"[{nameof(LevelService)}] Не удалось загрузить локацию '{levelData.Key}': {e.Message}");
			}
		}

		private void UnloadLevelLocation() {
			if (_hasActiveLevelLocationHandle) {
				Addressables.ReleaseInstance(_activeLevelLocationHandle);
				_hasActiveLevelLocationHandle = false;
			} else if (_activeLevelLocationInstance != null) {
				UnityEngine.Object.Destroy(_activeLevelLocationInstance);
			}

			_activeLevelLocationInstance = null;
		}
	}
}
