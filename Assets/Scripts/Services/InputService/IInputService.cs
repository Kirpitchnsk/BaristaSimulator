using UnityEngine;

namespace SibGameJam2026.Services {
	public interface IInputService {
		bool IsButtonPressed(string buttonName);
		bool WasButtonPressedThisFrame(string buttonName);
		Vector2 GetVector(string vectorName);
		void SwitchActionMap(string actionMapName);
	}
}
