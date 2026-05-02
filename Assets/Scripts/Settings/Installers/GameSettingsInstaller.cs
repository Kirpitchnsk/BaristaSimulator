using UnityEngine;
using Zenject;

namespace SibGameJam2026.Settings.Installers {
	[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
	public class GameSettingsInstaller : ScriptableObjectInstaller {
		[SerializeField] private GameSettingsData _gameSettingsData;

		public override void InstallBindings() {
			Container.Bind<GameSettingsData>().FromInstance(_gameSettingsData).AsSingle();
		}
	}
}
