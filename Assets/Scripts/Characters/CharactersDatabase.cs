using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Characters {
	[CreateAssetMenu(fileName = "CharactersDatabase", menuName = "Data/CharactersDatabase")]
	public class CharactersDatabase : ScriptableObjectInstaller<CharactersDatabase> {
		[SerializeField] private List<CharacterEntry> _entries = new();

		public override void InstallBindings() {
			Container.Bind<CharactersDatabase>().FromInstance(this).AsSingle();
		}

		public CharacterEntry GetEntry(ECharacterType eCharacterType) {
			var entry = _entries.Find(x => x.ECharacterType == eCharacterType);
			if (entry == null) {
				throw new ArgumentOutOfRangeException(nameof(eCharacterType), eCharacterType, "Character entry not found");
			}

			return entry;
		}
	}
}
