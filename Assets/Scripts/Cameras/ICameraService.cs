using UnityEngine;

namespace SibGameJam2026.Cameras {
	public interface ICameraService {
		CameraController CreateCamera(ECameraType cameraType, Transform parent);

		/// <summary>Создать игровую камеру один раз до появления игрока (временная точка привязки в мире).</summary>
		void EnsureGameplayCamera();

		/// <summary>Перепривязать активную камеру к точке игрока после спавна.</summary>
		void AttachActiveCameraTo(Transform followTarget);

		bool TryGetActiveCamera(out CameraController activeCamera);
	}
}
