using UnityEngine;
using Zenject;

namespace SibGameJam2026.Services {
	public class InventoryServiceInstaller : MonoInstaller {
		[SerializeField] private InventoryData _inventoryData;

		public override void InstallBindings() {
			Container.Bind<InventoryData>().FromInstance(_inventoryData).AsSingle();
			
			Container.Bind<IInventoryService>().To<PlayerInventoryService>().AsSingle();
		}
	}
}