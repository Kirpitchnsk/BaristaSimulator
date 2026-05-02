using System;
using SibGameJam2026.Services;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Cameras {
	public class CameraFactory : IFactory<ECameraType, Transform, CameraController> {
		private readonly CamerasDatabase _camerasDatabase;
		private readonly IInputService _inputService;

		public CameraFactory(CamerasDatabase camerasDatabase, IInputService inputService) {
			_camerasDatabase = camerasDatabase;
			_inputService = inputService;
		}

		public CameraController Create(ECameraType eCameraType, Transform parent) {
			if (parent == null)
				throw new ArgumentNullException(nameof(parent));

			var entry = _camerasDatabase.GetEntry(eCameraType);
			var cameraInstance = entry.CameraPrefab.InstantiateAsync().WaitForCompletion();
			var controller = cameraInstance.GetComponent<CameraController>();

			controller.Initialize(parent, _inputService);
			return controller;
		}
	}
}
