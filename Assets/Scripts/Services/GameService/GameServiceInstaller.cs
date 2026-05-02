using Zenject;

namespace SibGameJam2026.Services {
	public class GameServiceInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.Bind<ILevelService>().To<LevelService>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameService>().AsSingle().NonLazy();
		}
	}
}