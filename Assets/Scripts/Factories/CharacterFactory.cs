using System;
using System.Collections.Generic;
using SibGameJam2026.Cameras;
using SibGameJam2026.Characters.Components;
using SibGameJam2026.MergeService;
using SibGameJam2026.Services;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Characters {
	public class CharacterFactory : IFactory<ECharacterType, Vector3, ACharacter> {
		private readonly CharactersDatabase _charactersDatabase;
		private readonly IInputService _inputService;
		private readonly ICameraService _cameraService;
		/// <summary>Отложенный резолв: иначе цикл LevelService → Factory → ILevelService.</summary>
		private readonly LazyInject<ILevelService> _levelService;
		private readonly CarManager _carManager;
		private readonly ItemsFactory _itemsFactory;
		private readonly IInteractionSoundService _interactionSoundService;

		public CharacterFactory(
			CharactersDatabase charactersDatabase,
			IInputService inputService,
			ICameraService cameraService,
			LazyInject<ILevelService> levelService,
			CarManager carManager,
			ItemsFactory itemsFactory,
			IInteractionSoundService interactionSoundService
		) {
			_charactersDatabase = charactersDatabase;
			_inputService = inputService;
			_cameraService = cameraService;
			_levelService = levelService;
			_carManager = carManager;
			_itemsFactory = itemsFactory;
			_interactionSoundService = interactionSoundService;
		}

		public ACharacter Create(ECharacterType eCharacterType, Vector3 spawnPosition) {
			var entry = _charactersDatabase.GetEntry(eCharacterType);
			var prefabInstance = entry.CharacterPrefab.InstantiateAsync().WaitForCompletion();
			if (prefabInstance == null) {
				throw new InvalidOperationException($"Failed to instantiate prefab for character type {eCharacterType}");
			}

			var character = prefabInstance.GetComponent<ACharacter>();
			if (character == null)
				throw new InvalidOperationException($"Prefab for character type {eCharacterType} does not contain {nameof(ACharacter)}");

			character.transform.position = spawnPosition;
			
			character.Initialize(CreateComponents(entry, character));
			
			return character;
		}

		private IReadOnlyDictionary<Type, ICharacterComponent> CreateComponents(
			CharacterEntry entry,
			ACharacter character
		) {
			return entry.ECharacterType switch {
				ECharacterType.Player => CreatePlayerComponents(entry, character),
				_ => CreateClientComponents(entry, character),
			};
		}

		private IReadOnlyDictionary<Type, ICharacterComponent> CreatePlayerComponents(
			CharacterEntry entry,
			ACharacter character
		) {
			return new Dictionary<Type, ICharacterComponent> {
				{ typeof(IMovementCharacterComponent), new MovementCharacterComponent(character, entry) },
				{
					typeof(IInputCharacterComponent),
					new InputCharacterComponent(character, _inputService, _cameraService)
				},
				{
					typeof(IInteractableComponent),
					new InteractableCharacterComponent(character, _cameraService, _interactionSoundService)
				},
				{ typeof(IInventoryComponent), new SimpleCharacterInventoryComponent(character) }
			};
		}

		private IReadOnlyDictionary<Type, ICharacterComponent> CreateClientComponents(
			CharacterEntry entry,
			ACharacter character
		) {
			var authoring = new NpcControlStateAuthoring(_carManager);

			return new Dictionary<Type, ICharacterComponent> {
				{ typeof(IMovementCharacterComponent), new NpcMovementCharacterComponent(character, entry) },
				{
					typeof(INpcControlStateCharacterComponent),
					new NpcControlStateCharacterComponent(character, authoring)
				},
				{
					typeof(IInteractableCharacterComponent),
					new ClientInteractableCharacterComponent(character, _levelService.Value, _itemsFactory)
				},
			};
		}
	}
}
