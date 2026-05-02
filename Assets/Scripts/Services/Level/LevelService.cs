using System;
using System.Collections.Generic;
using Arenar.Services.UI;
using SibGameJam2026.Cameras;
using SibGameJam2026.Characters;
using SibGameJam2026.Characters.Components;
using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using SibGameJam2026.Settings;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Services {
	public class LevelService : ILevelService {
		private const string ClientsExhaustedLog = "[LevelService] Персонажи закончились";

		private readonly ACharacter.Factory _characterFactory;
		private readonly ICameraService _cameraService;
		private readonly GameSettingsData[] _gameSettingsData;
		private readonly ItemsDatabase _itemsDatabase;
		private readonly IMergeSystem _mergeSystem;
		private readonly CarManager _carManager;
		/// <summary>Отложенный резолв: контроллер появляется в контейнере только после <see cref="CanvasService.Initialize"/>.</summary>
		private readonly LazyInject<GameplayCanvasWindowController> _gameplayCanvasController;

		private readonly Dictionary<int, ItemId> _clientExpectedDishByInstanceId = new();

		private GameSettingsData _activeLevel;
		private int _nextClientIndex;
		private int _currentActiveClientIndex = -1;
		private int _cookSuccessCount;
		private int _cookFailureCount;

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

			_nextClientIndex = 0;
			_currentActiveClientIndex = -1;
			_cookSuccessCount = 0;
			_cookFailureCount = 0;

			_clientExpectedDishByInstanceId.Clear();

			if (_activeLevel == null)
				Debug.LogWarning($"[{nameof(LevelService)}] Уровень не найден (ключ '{levelKey}'). Очередь NPC не запущена.");
			else
				ResetGameplayUi(_activeLevel);

			var player = _characterFactory.Create(ECharacterType.Player, Vector3.zero);
			var cameraParent = player.Data.CameraPoint != null ? player.Data.CameraPoint : player.transform;
			_cameraService.AttachActiveCameraTo(cameraParent);

			TrySpawnNextClientNpc();
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

		public void SetCookFailed() {
			if (_currentActiveClientIndex < 0)
				return;

			var list = _activeLevel?.ClientData;
			if (list == null || _currentActiveClientIndex >= list.Count)
				return;

			_cookFailureCount++;
			_gameplayCanvasController.Value?.SetClientCookFailed(_currentActiveClientIndex);
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
				void Handler(EClientState state) {
					if (state != EClientState.Finished)
						return;
					npcState.StateChanged -= Handler;
					TrySpawnNextClientNpc();
				}

				npcState.StateChanged += Handler;
			} else {
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
	}
}
