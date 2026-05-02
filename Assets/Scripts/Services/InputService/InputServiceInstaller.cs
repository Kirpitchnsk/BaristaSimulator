using Zenject;

namespace SibGameJam2026.Services {
	public class InputServiceInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.Bind<IInputService>().To<UnityInputService>().AsSingle();
		}
	}
}
