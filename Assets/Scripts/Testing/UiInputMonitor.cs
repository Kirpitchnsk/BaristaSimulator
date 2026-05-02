using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SibGameJam2026.Testing {
	/// <summary>
	/// Subscribes to all actions in the <c>UI</c> map and logs phase changes (editor / dev builds).
	/// If you see <c>UI/Click</c> here but buttons stay dead, the problem is likely raycasts / EventSystem routing, not the action asset.
	/// </summary>
	public sealed class UiInputMonitor : IInitializable, IDisposable {
		private const string UiMapName = "UI";

		private readonly InputActionAsset _asset;
		private readonly List<InputAction> _hooked = new();

		[Inject]
		public UiInputMonitor(InputActionAsset asset) {
			_asset = asset;
		}

		public void Initialize() {
			var ui = _asset.FindActionMap(UiMapName, false);
			if (ui == null) {
				Debug.LogWarning($"[{nameof(UiInputMonitor)}] Action map '{UiMapName}' not found on {_asset.name}.");
				return;
			}

			foreach (var action in ui.actions) {
				action.started += OnUiPhase;
				action.performed += OnUiPhase;
				action.canceled += OnUiPhase;
				_hooked.Add(action);
			}
		}

		public void Dispose() {
			foreach (var action in _hooked) {
				action.started -= OnUiPhase;
				action.performed -= OnUiPhase;
				action.canceled -= OnUiPhase;
			}

			_hooked.Clear();
		}

		private static void OnUiPhase(InputAction.CallbackContext ctx) {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var action = ctx.action;
			if (action == null)
				return;

			// Point спамит каждый кадр — по умолчанию не логируем, только клики/кнопки навигации.
			if (action.name == "Point")
				return;

			Debug.Log($"[{nameof(UiInputMonitor)}] {action.actionMap?.name}/{action.name} phase={ctx.phase} value={ctx.ReadValueAsObject()}");
#endif
		}
	}
}
