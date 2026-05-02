using UnityEngine;
using Zenject;

namespace SibGameJam2026.Testing {
	public class TestInstaller : MonoInstaller {
		[SerializeField] private bool _enableUiInputMonitor = true;

		public override void InstallBindings() {
			if (_enableUiInputMonitor)
				Container.BindInterfacesAndSelfTo<UiInputMonitor>().AsSingle().NonLazy();
		}
	}
}
