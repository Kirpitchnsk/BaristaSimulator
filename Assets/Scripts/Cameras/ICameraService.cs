using UnityEngine;

namespace SibGameJam2026.Cameras {
	public interface ICameraService {
		CameraController CreateCamera(ECameraType cameraType, Transform parent);
		bool TryGetActiveCamera(out Camera activeCamera);
	}
}
