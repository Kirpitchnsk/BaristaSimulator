using UnityEngine;

namespace SibGameJam2026.Services {
	public interface IInputService {
		bool IsButtonPressed(string buttonName);
		bool WasButtonPressedThisFrame(string buttonName);
		Vector2 GetVector(string vectorName);

		/// <summary>
		/// Look/camera rotation: mouse delta (pixels per frame) and stick (normalized) use different scaling — mouse must not be multiplied by <paramref name="deltaTime"/>.
		/// </summary>
		Vector2 GetCameraLook(string actionName, float mouseSensitivity, float stickSensitivity, float deltaTime);

		void SwitchActionMap(string actionMapName);

		/// <summary>Disables gameplay maps and leaves only the <c>UI</c> map (for menus).</summary>
		void SwitchToUIMap();
	}
}
