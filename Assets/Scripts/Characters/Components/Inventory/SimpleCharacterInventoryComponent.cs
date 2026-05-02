using SibGameJam2026.MergeService;
using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	public class SimpleCharacterInventoryComponent : IInventoryComponent {
		private readonly ACharacter _character;
		private ItemVisual _itemVisual;

		public ACharacter Character => _character;
		public bool HasItem => _itemVisual != null;
		public ItemVisual ItemVisual => _itemVisual;
		public Item CurrentItem => _itemVisual != null ? _itemVisual.ItemData : default;

		public SimpleCharacterInventoryComponent(ACharacter character) {
			_character = character;
		}

		public bool TryStoreItem(ItemVisual itemVisual) {
			if (itemVisual == null || HasItem) {
				return false;
			}

			_itemVisual = itemVisual;

			var targetParent = _character.Data.ItemPosition != null
				? _character.Data.ItemPosition
				: _character.transform;

			var itemTransform = itemVisual.transform;
			itemTransform.SetParent(targetParent, false);
			itemTransform.localPosition = Vector3.zero;
			itemTransform.localRotation = Quaternion.identity;
			return true;
		}

		public bool TryTakeItem(out ItemVisual itemVisual) {
			if (_itemVisual == null) {
				itemVisual = null;
				return false;
			}

			itemVisual = _itemVisual;
			_itemVisual = null;
			return true;
		}
	}
}
