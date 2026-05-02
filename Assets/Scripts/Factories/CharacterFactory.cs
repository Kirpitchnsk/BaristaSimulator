using System;
using System.Collections.Generic;
using SibGameJam2026.Cameras;
using SibGameJam2026.Characters.Components;
using SibGameJam2026.Services;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Characters {
	public class CharacterFactory : IFactory<ECharacterType, Vector3, ACharacter> {
		private readonly CharactersDatabase _charactersDatabase;
		private readonly IInputService _inputService;
		private readonly ICameraService _cameraService;

		public CharacterFactory(CharactersDatabase charactersDatabase, IInputService inputService, ICameraService cameraService) {
			_charactersDatabase = charactersDatabase;
			_inputService = inputService;
			_cameraService = cameraService;
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

		private IReadOnlyDictionary<Type, ICharacterComponent> CreateComponents(CharacterEntry entry, ACharacter character) {
			return entry.ECharacterType switch {
				ECharacterType.Player => new Dictionary<Type, ICharacterComponent> {
					{ typeof(IHealthCharacterComponent), new HealthCharacterComponent(character, entry.Health) },
					{ typeof(IMovementCharacterComponent), new MovementCharacterComponent(character, entry) },
					{ typeof(IInputCharacterComponent), new InputCharacterComponent(character, _inputService, _cameraService) },
					{ typeof(IInteractableComponent), new InteractableCharacterComponent(character, _cameraService) },
					{ typeof(IInventoryComponent), new SimpleCharacterInventoryComponent(character) }
				},
				_ => throw new ArgumentOutOfRangeException(nameof(entry.ECharacterType), entry.ECharacterType, "Unknown character type")
			};
		}
	}
}
