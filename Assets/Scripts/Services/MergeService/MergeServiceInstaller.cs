using Zenject;

namespace SibGameJam2026.MergeService {
	public class MergeServiceInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.Bind<IMergeSystem>().To<MergeSystem>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<ItemsFactory>().AsSingle();
		}
	}
}
