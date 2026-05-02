using UnityEngine;

namespace SibGameJam2026 {
	public class ItemMergerActivator : AInteractItemVisual {
		[SerializeField] private ItemMerger itemMerger;
		
		public override void OnInteract(InteractContext context) {
			itemMerger.TryStartMergeProcess();
		}
	}
}