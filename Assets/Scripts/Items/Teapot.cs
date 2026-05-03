using UnityEngine;

namespace SibGameJam2026 {
	public class Teapot : ItemTransformer {
		private void Reset() {
			_interactionSound = ESoundType.Teapot;
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (_interactionSound == ESoundType.None)
				_interactionSound = ESoundType.Teapot;
		}
#endif
	}
}
