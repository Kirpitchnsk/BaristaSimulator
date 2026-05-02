using SibGameJam2026.Characters.Components;
using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using UnityEngine;
using Zenject;

namespace SibGameJam2026 {
	public class ItemSpawner : AInteractItemVisual {
		[SerializeField] private ItemId _itemId;

		private ItemsFactory _itemsFactory;

		[Inject]
		private void Construct(ItemsFactory itemsFactory) {
			_itemsFactory = itemsFactory;
		}

		public override void OnInteract(InteractContext context) {
			if (_itemsFactory == null)
				return;

			if (!context.UserCharacter.TryGetComponent<IInventoryComponent>(
				out var inventoryComponent))
				return;

			if (inventoryComponent.HasItem)
				return;

			var itemPosition = context.UserCharacter.Data.ItemPosition;
			var spawnTransform = itemPosition != null ? itemPosition : context.UserCharacter.transform;
			var itemVisual = _itemsFactory.Create(_itemId, spawnTransform.position, spawnTransform.rotation);

			if (!inventoryComponent.TryStoreItem(itemVisual))
				_itemsFactory.ReturnToPool(itemVisual);
		}
	}
}
