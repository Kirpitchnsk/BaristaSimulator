using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SibGameJam2026.Services {
	public class UnityInputService : IInputService, IInitializable {
		private const string UiMapName = "UI";

		private readonly InputActionAsset _asset;

		[Inject]
		public UnityInputService(InputActionAsset gameInputAsset) {
			_asset = gameInputAsset;
		}

		public void Initialize() => SwitchToUIMap();

		public void SwitchToUIMap() => SwitchActionMap(UiMapName);

		public bool IsButtonPressed(string buttonName) {
			var action = FindAction(buttonName);
			return action != null && action.IsPressed();
		}

		public bool WasButtonPressedThisFrame(string buttonName) {
			var action = FindAction(buttonName);
			return action != null && action.WasPressedThisFrame();
		}

		public Vector2 GetVector(string vectorName) {
			var action = FindAction(vectorName);
			return action != null ? action.ReadValue<Vector2>() : Vector2.zero;
		}

		public void SwitchActionMap(string actionMapName) {
			if (string.IsNullOrWhiteSpace(actionMapName))
				return;

			var uiMap = _asset.FindActionMap(UiMapName, false);
			var targetMap = _asset.FindActionMap(actionMapName, false);
			if (targetMap == null)
				return;

			// Never disable the UI map here: InputSystemUIInputModule keeps references to UI actions.
			// Disabling the whole UI map breaks pointer routing until the module is reset.
			foreach (var map in _asset.actionMaps) {
				if (map == uiMap)
					continue;
				map.Disable();
			}

			if (targetMap != uiMap)
				targetMap.Enable();

			uiMap?.Enable();
		}

		private InputAction FindAction(string actionName) {
			if (string.IsNullOrWhiteSpace(actionName))
				return null;

			return _asset.FindAction(actionName, false);
		}
	}
}
