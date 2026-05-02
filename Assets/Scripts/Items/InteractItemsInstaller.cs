using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SibGameJam2026 {
	public class InteractItemsInstaller : MonoInstaller {
		[SerializeField] private List<AInteractItemVisual> _interactItemVisuals = new();

		public override void InstallBindings() {
			for (var i = 0; i < _interactItemVisuals.Count; i++) {
				var interactItemVisual = _interactItemVisuals[i];
				if (interactItemVisual == null) {
					continue;
				}

				Container.QueueForInject(interactItemVisual);
			}
		}
	}
}
