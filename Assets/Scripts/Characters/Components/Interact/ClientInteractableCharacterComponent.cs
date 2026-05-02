using SibGameJam2026.Services;
using SibGameJam2026.MergeService;
using UnityEngine;

namespace SibGameJam2026.Characters.Components {
	public class ClientInteractableCharacterComponent : IInteractableCharacterComponent {
		private readonly ACharacter _character;
		private readonly ILevelService _levelService;
		private readonly ItemsFactory _itemsFactory;
		
		private INpcControlStateCharacterComponent _npcState;

		public ACharacter Character => _character;

		public ClientInteractableCharacterComponent(
			ACharacter character,
			ILevelService levelService,
			ItemsFactory itemsFactory
		) {
			_character = character;
			_levelService = levelService;
			_itemsFactory = itemsFactory;
		}

		public void OnInteract(InteractContext context) {
			if (_npcState == null) {
				if (!_character.TryGetComponent<INpcControlStateCharacterComponent>(out _npcState))
					return;
			}
			
			switch (_npcState.State) {
				case EClientState.WaitInteraction:
					_levelService.AssignCookingDishForClient(_character);
					_levelService.PresentClientOrderUi(_character);
					_npcState.SetState(EClientState.WaitCooking);
					break;
				case EClientState.WaitCooking:
					if (!_levelService.TryGetExpectedDish(_character, out var expectedDish)) {
						D.Error($"[{nameof(ClientInteractableCharacterComponent)}] No expected dish for {_character.name}.");
						return;
					}

					var served = context.UsedItem;
					var isSuccess = served.ItemId == expectedDish;
					_npcState.SetState(isSuccess ? EClientState.TransformCreatureSuccess : EClientState.TransformCreatureFailed);

					if (context.UserCharacter.TryGetComponent<IInventoryComponent>(out var userInventory)
					    && userInventory.TryTakeItem(out var itemVisual)) {
						_itemsFactory.ReturnToPool(itemVisual);
					}

					D.Log(isSuccess
						? $"[{nameof(ClientInteractableCharacterComponent)}] Correct item served: {served.Name} ({served.ItemId})."
						: $"[{nameof(ClientInteractableCharacterComponent)}] Wrong item: served {served.ItemId}, expected {expectedDish}.");
					break;
				default:
					return;
			}
		}
	}
}
