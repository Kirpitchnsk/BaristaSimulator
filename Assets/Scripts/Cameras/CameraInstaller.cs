using UnityEngine;
using Zenject;

namespace SibGameJam2026.Cameras {
	public class CameraInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.BindFactory<ECameraType, Transform, CameraController, CameraController.Factory>().FromFactory<CameraFactory>();
			Container.Bind<ICameraService>().To<CameraService>().AsSingle();
		}
	}
}
