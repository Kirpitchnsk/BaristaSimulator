using SibGameJam2026.Services;
using UnityEngine;
using Zenject;

namespace SibGameJam2026 {
	public class CoffeeMachine : ItemMerger {
		private IInteractionSoundService _interactionSounds;

		[Inject]
		private void ConstructSounds(IInteractionSoundService interactionSoundService) {
			_interactionSounds = interactionSoundService;
		}

		protected override void CompleteProcessing() {
			base.CompleteProcessing();
			_interactionSounds?.Play(gameObject, ESoundType.CoffeeMachineComplete);
		}

		private void Reset() {
			_interactionSound = ESoundType.CoffeeMachine;
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (_interactionSound == ESoundType.None)
				_interactionSound = ESoundType.CoffeeMachine;
		}
#endif
	}
}
