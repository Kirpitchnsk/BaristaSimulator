using SibGameJam2026.Characters.Components;
using SibGameJam2026.MergeService;
using Zenject;

namespace SibGameJam2026 {
	public class ItemDisposer : AInteractItemVisual {
		private ItemsFactory _itemsFactory;

		[Inject]
		private void Construct(ItemsFactory itemsFactory) 
			=> _itemsFactory = itemsFactory;

		public override string InteractItemName => "Мусорка";

		public override void OnInteract(InteractContext context) {
			if (_itemsFactory == null)
				return;

			if (!context.UserCharacter.TryGetComponent<IInventoryComponent>(out var inventoryComponent))
				return;

			if (inventoryComponent.TryTakeItem(out var itemVisual))
				_itemsFactory.ReturnToPool(itemVisual);
		}
	}
}
