using System.Collections.Generic;
using SibGameJam2026.Items;
using SibGameJam2026.MergeService;

namespace Arenar.Services.UI {
    public class CookMenuCanvasWindowLayer : CanvasWindowLayer
    {
	    public ItemContainerUiVisual ResultItem;
	    public ItemContainerUiVisual[] NeededItems;

	    public void SetLayerEnabled(bool isEnabled) {
		    gameObject.SetActive(isEnabled);
	    }

	    public void ApplyRecipe(ItemsDatabase itemsDatabase, ItemId resultId, IReadOnlyList<ItemId> ingredientIds) {
		    if (ResultItem != null) {
			    if (itemsDatabase != null && itemsDatabase.TryGetItemByItemId(resultId, out var result))
				    ResultItem.SetItem(result.Icon, result.Name);
			    else
				    ResultItem.SetItem(null, resultId.ToString());
		    }

		    if (NeededItems == null)
			    return;

		    var count = ingredientIds?.Count ?? 0;
		    for (var i = 0; i < NeededItems.Length; i++) {
			    var slot = NeededItems[i];
			    if (slot == null)
				    continue;

			    if (i < count) {
				    var id = ingredientIds[i];
				    slot.gameObject.SetActive(true);
				    if (itemsDatabase != null && itemsDatabase.TryGetItemByItemId(id, out var ing))
					    slot.SetItem(ing.Icon, ing.Name);
				    else
					    slot.SetItem(null, id.ToString());
			    } else {
				    slot.gameObject.SetActive(false);
			    }
		    }
	    }
    }
}