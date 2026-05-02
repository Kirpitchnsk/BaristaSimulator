using System.Collections.Generic;
using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using SibGameJam2026.Services;
using SibGameJam2026.Settings;
using UnityEngine;
using Zenject;

namespace Arenar.Services.UI {
	public class GameplayCanvasWindowController : CanvasWindowController {
		private GameplayCanvasWindow _gameplayCanvasWindow;
		private CookMenuCanvasWindowLayer _cookMenuLayer;
		private TimerCanvasWindowLayer _timerLayer;
		private DialogAnswersCanvasWindowLayer _dialogAnswersLayer;
		private ClientsListCanvasWindowLayer _clientsListLayer;
		private readonly ItemsDatabase _itemsDatabase;

		public GameplayCanvasWindowController(
			IInputService inputService,
			[InjectOptional] ItemsDatabase itemsDatabase
		) : base(inputService) {
			_itemsDatabase = itemsDatabase;
		}

		public override void Initialize(ICanvasService canvasService) {
			base.Initialize(canvasService);

			_gameplayCanvasWindow = canvasService.GetWindow<GameplayCanvasWindow>();
			_cookMenuLayer = _gameplayCanvasWindow.GetWindowLayer<CookMenuCanvasWindowLayer>();
			_timerLayer = _gameplayCanvasWindow.GetWindowLayer<TimerCanvasWindowLayer>();
			_dialogAnswersLayer = _gameplayCanvasWindow.GetWindowLayer<DialogAnswersCanvasWindowLayer>();
			_clientsListLayer = _gameplayCanvasWindow.GetWindowLayer<ClientsListCanvasWindowLayer>();
		}

		public void SetupForLevel(GameSettingsData gameData) {
			if (_gameplayCanvasWindow == null) {
				Debug.LogWarning($"[{nameof(GameplayCanvasWindowController)}] Gameplay window is not initialized.");
				return;
			}

			_cookMenuLayer?.SetLayerEnabled(false);
			_timerLayer?.SetLayerEnabled(false);
			_dialogAnswersLayer?.SetLayerEnabled(false);
			_clientsListLayer?.SetClients(gameData);
		}

		public void SetClientCookFailed(int slotIndex) {
			_clientsListLayer?.SetSlotCookFailed(slotIndex);
		}

		public void ShowCookMenu(ItemId resultDishId, IReadOnlyList<ItemId> ingredientIds) {
			if (_cookMenuLayer == null) {
				Debug.LogWarning($"[{nameof(GameplayCanvasWindowController)}] Cook menu layer is missing.");
				return;
			}

			_cookMenuLayer.SetLayerEnabled(true);
			_cookMenuLayer.ApplyRecipe(_itemsDatabase, resultDishId, ingredientIds);
		}

		protected override void OnWindowShowEnd_SelectElements() {
		}

		protected override void OnWindowHideBegin_DeselectElements() {
		}
	}
}
