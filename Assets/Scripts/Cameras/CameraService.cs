using UnityEngine;

namespace SibGameJam2026.Cameras {
	public class CameraService : ICameraService {
		private readonly CameraController.Factory _cameraFactory;
		private CameraController _activeCameraController;

		public CameraService(CameraController.Factory cameraFactory) {
			_cameraFactory = cameraFactory;
		}

		public CameraController CreateCamera(ECameraType cameraType, Transform parent) {
			_activeCameraController = _cameraFactory.Create(cameraType, parent);
			return _activeCameraController;
		}

		public bool TryGetActiveCamera(out Camera activeCamera) {
			activeCamera = null;

			if (_activeCameraController == null) {
				return false;
			}

			activeCamera = _activeCameraController.GetComponent<Camera>();
			return activeCamera != null;
		}
	}
}
