using System.Collections.Generic;
using SibGameJam2026.Items;

namespace SibGameJam2026.MergeService {
	public interface IMergeSystem {
		bool TryGetMergedProductId(IReadOnlyList<ItemId> inputProductIds, out ItemId outputProductId);

		bool TryGetSourceProductIds(ItemId outputProductId, out IReadOnlyList<ItemId> sourceProductIds);
	}
}
