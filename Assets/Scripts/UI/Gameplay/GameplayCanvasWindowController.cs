using System.Collections.Generic;
using SibGameJam2026;
using SibGameJam2026.Cameras;
using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using SibGameJam2026.Services;
using SibGameJam2026.Settings;
using UnityEngine;
using Zenject;

namespace Arenar.Services.UI {
	public class GameplayCanvasWindowController : CanvasWindowController {
		private const float ItemRaycastDistance = 2f;
		private static readonly int ItemLayerMask = LayerMask.GetMask("Item");

		private GameplayCanvasWindow _gameplayCanvasWindow;
		
		private CookMenuCanvasWindowLayer _cookMenuLayer;
		private TimerCanvasWindowLayer _timerLayer;
		private GameplayMarkCanvasWindowLayer _markLayer;
		private DialogAnswersCanvasWindowLayer _dialogAnswersLayer;
		private ClientsListCanvasWindowLayer _clientsListLayer;
		
		private readonly ItemsDatabase _itemsDatabase;
		private readonly ICameraService _cameraService;

		public GameplayCanvasWindowController(
			IInputService inputService,
			ICameraService cameraService,
			[InjectOptional] ItemsDatabase itemsDatabase
		) : base(inputService) {
			_cameraService = cameraService;
			_itemsDatabase = itemsDatabase;
		}

		public override void Initialize(ICanvasService canvasService) {
			base.Initialize(canvasService);

			_gameplayCanvasWindow = canvasService.GetWindow<GameplayCanvasWindow>();
			_cookMenuLayer = _gameplayCanvasWindow.GetWindowLayer<CookMenuCanvasWindowLayer>();
			_timerLayer = _gameplayCanvasWindow.GetWindowLayer<TimerCanvasWindowLayer>();
			_markLayer = _gameplayCanvasWindow.GetWindowLayer<GameplayMarkCanvasWindowLayer>();
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
			_markLayer?.SetVisible(false);
			_dialogAnswersLayer?.SetLayerEnabled(false);
			_clientsListLayer?.SetClients(gameData);
		}

		public void UpdateFocusItemMark() {
			if (_markLayer == null)
				return;
			if (!_cameraService.TryGetActiveCamera(out var controller)) {
				_markLayer.SetVisible(false);
				return;
			}

			var activeCamera = controller.GetComponent<Camera>();
			if (activeCamera == null) {
				_markLayer.SetVisible(false);
				return;
			}

			var ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (!Physics.Raycast(ray, out var hit, ItemRaycastDistance, ItemLayerMask)) {
				_markLayer.SetVisible(false);
				return;
			}

			IInteractable itemVisual = hit.collider.GetComponent<IInteractable>();
			if (itemVisual == null) {
				_markLayer.SetVisible(false);
				return;
			}

			_markLayer.SetVisible(true);
			_markLayer.SetItemName(itemVisual.InteractItemName);
		}

		public void SetClientCookFailed(int slotIndex) {
			_clientsListLayer?.SetSlotCookFailed(slotIndex);
		}

		public void SetClientCookSuccess(int slotIndex) {
			_clientsListLayer?.SetSlotCookSuccess(slotIndex);
		}

		public void SetCookTimerActive(bool isActive) {
			_timerLayer?.SetLayerEnabled(isActive);
			if (!isActive)
				_timerLayer?.SetTimerProgress(0f, 1f);
		}

		public void UpdateCookTimer(float remainingSeconds, float maxSeconds) {
			if (_timerLayer == null)
				return;

			_timerLayer.SetTimerProgress(remainingSeconds, maxSeconds);
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
