using Zenject;

namespace SibGameJam2026.Services {
	public class GameServiceInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<GameService>().AsSingle().NonLazy();
		}
	}
}