using System;
using UnityEngine;

namespace SibGameJam2026.Cameras {
	public class CameraService : ICameraService {
		private readonly CameraController.Factory _cameraFactory;
		private CameraController _activeCameraController;
		private Transform _cameraStagingRoot;

		public CameraService(CameraController.Factory cameraFactory) {
			_cameraFactory = cameraFactory;
		}

		public CameraController CreateCamera(ECameraType cameraType, Transform parent) {
			_activeCameraController = _cameraFactory.Create(cameraType, parent);
			return _activeCameraController;
		}

		public void EnsureGameplayCamera() {
			if (_activeCameraController != null)
				return;

			CreateCamera(ECameraType.FirstPerson, GetOrCreateStagingRoot());
		}

		public void AttachActiveCameraTo(Transform followTarget) {
			if (followTarget == null)
				throw new ArgumentNullException(nameof(followTarget));
			if (_activeCameraController == null)
				throw new InvalidOperationException("Call EnsureGameplayCamera before AttachActiveCameraTo.");

			_activeCameraController.SetFollowTarget(followTarget);
		}

		public bool TryGetActiveCamera(out CameraController activeCamera) {
			activeCamera = _activeCameraController;
			return activeCamera != null;
		}

		private Transform GetOrCreateStagingRoot() {
			if (_cameraStagingRoot != null)
				return _cameraStagingRoot;

			var go = new GameObject(nameof(CameraService) + "_CameraStaging");
			_cameraStagingRoot = go.transform;
			_cameraStagingRoot.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			return _cameraStagingRoot;
		}
	}
}
