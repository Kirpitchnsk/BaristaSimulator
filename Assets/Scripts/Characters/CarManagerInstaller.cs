using UnityEngine;
using Zenject;

namespace SibGameJam2026.Characters {
	public class CarManagerInstaller : MonoInstaller {
		[SerializeField] private CarManager _carManager;

		public override void InstallBindings() {
			Container.Bind<CarManager>().FromInstance(_carManager).AsSingle().NonLazy();
		}
	}
}
