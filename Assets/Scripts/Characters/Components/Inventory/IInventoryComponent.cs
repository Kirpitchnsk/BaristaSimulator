using SibGameJam2026.MergeService;

namespace SibGameJam2026.Characters.Components {
	public interface IInventoryComponent : ICharacterComponent {
		bool HasItem { get; }
		ItemVisual ItemVisual { get; }
		Item CurrentItem { get; }

		bool TryStoreItem(ItemVisual itemVisual);
		bool TryTakeItem(out ItemVisual itemVisual);
	}
}
