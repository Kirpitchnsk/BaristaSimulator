using Zenject;

namespace SibGameJam2026.Characters {
	public class CharacterInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.BindFactory<ECharacterType, UnityEngine.Vector3, ACharacter, ACharacter.Factory>()
				.FromFactory<CharacterFactory>().NonLazy();
		}
	}
}
