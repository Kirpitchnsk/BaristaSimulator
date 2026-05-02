using UnityEngine.InputSystem;
using SibGameJam2026.Input;
using UnityEngine;

namespace SibGameJam2026.Services {
	public class UnityInputService : IInputService {
		private readonly GameInput _gameInput;

		public UnityInputService() {
			_gameInput = new GameInput();
			_gameInput.Enable();
		}

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

			var actionMap = _gameInput.asset.FindActionMap(actionMapName, false);
			if (actionMap == null)
				return;

			foreach (var map in _gameInput.asset.actionMaps)
				map.Disable();

			actionMap.Enable();
		}

		private InputAction FindAction(string actionName) {
			if (string.IsNullOrWhiteSpace(actionName))
				return null;

			return _gameInput.asset.FindAction(actionName, false);
		}
	}
}
