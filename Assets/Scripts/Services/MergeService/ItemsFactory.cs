using System;
using System.Collections.Generic;
using SibGameJam2026.Items;
using UnityEngine;

namespace SibGameJam2026.MergeService {
	public class ItemsFactory {
		private readonly ItemsDatabase _itemsDatabase;
		private readonly Dictionary<ItemId, Queue<ItemVisual>> _poolByItemId = new();
		private readonly Dictionary<ItemVisual, ItemId> _reversePoolKey = new();
		private readonly Transform _poolRoot;

		public ItemsFactory(ItemsDatabase itemsDatabase) {
			_itemsDatabase = itemsDatabase;
			var poolRootObject = new GameObject("[ItemsFactoryPool]");
			_poolRoot = poolRootObject.transform;
			UnityEngine.Object.DontDestroyOnLoad(poolRootObject);
		}

		public ItemVisual Create(ItemId itemId, Vector3 position, Quaternion rotation, Transform parent = null) {
			if (!_itemsDatabase.TryGetItemByItemId(itemId, out var item)) {
				throw new ArgumentOutOfRangeException(nameof(itemId), (string)itemId, "Item was not found in ItemsDatabase.");
			}

			if (_poolByItemId.TryGetValue(itemId, out var pool) && pool.Count > 0) {
				var pooledVisual = pool.Dequeue();
				ActivateVisual(pooledVisual, position, rotation, parent);
				return pooledVisual;
			}

			if (item.Prefab == null) {
				throw new InvalidOperationException($"Prefab is not assigned for item {item.Name}({item.ItemId}).");
			}

			var visualObject = item.Prefab.InstantiateAsync(position, rotation, parent).WaitForCompletion();
			if (visualObject == null) {
				throw new InvalidOperationException($"Failed to instantiate visual for item {item.Name}({item.ItemId}).");
			}

			var visual = visualObject.GetComponent<ItemVisual>();
			if (visual == null) {
				visual = visualObject.AddComponent<ItemVisual>();
			}

			visual.Initialize(item);
			_reversePoolKey[visual] = itemId;
			return visual;
		}

		public void ReturnToPool(ItemVisual itemVisual) {
			if (itemVisual == null) {
				return;
			}

			if (!_reversePoolKey.TryGetValue(itemVisual, out var itemId)) {
				UnityEngine.Object.Destroy(itemVisual.gameObject);
				return;
			}

			if (!_poolByItemId.TryGetValue(itemId, out var pool)) {
				pool = new Queue<ItemVisual>();
				_poolByItemId[itemId] = pool;
			}

			itemVisual.transform.SetParent(_poolRoot, false);
			itemVisual.gameObject.SetActive(false);
			pool.Enqueue(itemVisual);
		}

		private static void ActivateVisual(ItemVisual visual, Vector3 position, Quaternion rotation, Transform parent) {
			visual.transform.SetParent(parent, false);
			visual.transform.SetPositionAndRotation(position, rotation);
			visual.SetInteractionColliderEnabled(true);
			visual.gameObject.SetActive(true);
		}
	}
}
