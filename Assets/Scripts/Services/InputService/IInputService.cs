using UnityEngine;

namespace SibGameJam2026.Services {
	public interface IInputService {
		bool IsButtonPressed(string buttonName);
		bool WasButtonPressedThisFrame(string buttonName);
		Vector2 GetVector(string vectorName);
		void SwitchActionMap(string actionMapName);

		/// <summary>Disables gameplay maps and leaves only the <c>UI</c> map (for menus).</summary>
		void SwitchToUIMap();
	}
}
