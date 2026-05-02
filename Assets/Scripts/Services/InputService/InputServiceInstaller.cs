using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Zenject;

namespace SibGameJam2026.Services {
	public class InputServiceInstaller : MonoInstaller {
		[SerializeField] private InputActionAsset _gameInputAsset;

		public override void InstallBindings() {
			Assert.IsNotNull(_gameInputAsset, "Assign GameInput (Input Action Asset) on InputServiceInstaller.");
			Container.Bind<InputActionAsset>().FromInstance(_gameInputAsset).AsSingle();
			Container.Bind<IInputService>().To<UnityInputService>().AsSingle();
		}
	}
}
