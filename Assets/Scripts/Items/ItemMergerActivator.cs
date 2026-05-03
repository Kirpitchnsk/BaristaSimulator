using UnityEngine;

namespace SibGameJam2026 {
	public class ItemMergerActivator : AInteractItemVisual {
		[SerializeField] private string _name;
		[SerializeField] private ItemMerger itemMerger;
		
		public override string InteractItemName => itemMerger.ItemsCount > 0 ? _name : "";

		public override void OnInteract(InteractContext context) {
			itemMerger.TryStartMergeProcess();
		}
	}
}