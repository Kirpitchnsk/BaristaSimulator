using SibGameJam2026.Characters;
using SibGameJam2026.MergeService;

namespace SibGameJam2026 {
	public readonly struct InteractContext {
		public ACharacter UserCharacter { get; }
		public Item UsedItem { get; }

		public InteractContext(ACharacter userCharacter, Item usedItem) {
			UserCharacter = userCharacter;
			UsedItem = usedItem;
		}
	}
}
