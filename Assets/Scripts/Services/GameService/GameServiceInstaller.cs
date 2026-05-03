using Zenject;

namespace SibGameJam2026.Services {
	public class GameServiceInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<LevelService>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameService>().AsSingle().NonLazy();
			Container.Bind<IInteractionSoundService>().To<InteractionSoundService>().AsSingle();
		}
	}
}