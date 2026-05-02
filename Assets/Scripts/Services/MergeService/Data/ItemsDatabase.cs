using System.Collections.Generic;
using SibGameJam2026.Items;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.MergeService {
	[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Data/ItemsDatabase", order = 54)]
	public class ItemsDatabase : ScriptableObjectInstaller<ItemsDatabase> {
		[SerializeField] private List<Item> _items = new();

		public IReadOnlyList<Item> Items => _items;

		public override void InstallBindings() {
			Container.Bind<ItemsDatabase>().FromInstance(this).AsSingle();
		}

		public bool TryGetItemByItemId(ItemId itemId, out Item item) {
			for (var i = 0; i < _items.Count; i++) {
				if (_items[i].ItemId != itemId) {
					continue;
				}

				item = _items[i];
				return true;
			}

			item = default;
			return false;
		}
	}
}
