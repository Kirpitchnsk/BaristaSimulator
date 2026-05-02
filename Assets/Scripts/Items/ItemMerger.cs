using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using SibGameJam2026.Characters.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace SibGameJam2026 {
	public abstract class ItemMerger : AInteractItemVisual {
		[SerializeField] private List<EItemType> _supportedInputTypes;
		[SerializeField] private int _maxBufferedItems = 3;
		[SerializeField] private float _processingTimeSeconds = 5f;

		[Space(10)]
		[SerializeField] private Transform[] _inputItemPositions;
		[SerializeField] private Transform _outputItemPosition;

		[Header("Processing visual (Coffee machine, etc.)")]
		[SerializeField] private Transform _processingVisualTransform;
		[SerializeField] private float _squashHalfCycleSeconds = 0.35f;
		[SerializeField] private float _squashXZStretch = 1.06f;
		[SerializeField] private float _squashYMul = 0.88f;

		private readonly List<Item> _bufferedItems = new();
		private readonly List<ItemVisual> _bufferedItemVisuals = new();

		private IMergeSystem _mergeSystem;
		private ItemsFactory _itemsFactory;
		private ItemsDatabase _itemsDatabase;
		private ItemId _processingOutputItemId;
		private string _processingInputInfo;
		private bool _isProcessing;
		private float _remainingTime;
		private ItemProcessingSquashState _squashState;

		[Inject]
		private void Construct(IMergeSystem mergeSystem, ItemsFactory itemsFactory, ItemsDatabase itemsDatabase) {
			_mergeSystem = mergeSystem;
			_itemsFactory = itemsFactory;
			_itemsDatabase = itemsDatabase;
		}

		public override void OnInteract(InteractContext context) {
			if (_isProcessing) {
				D.Log($"{nameof(ItemMerger)} is busy.");
				return;
			}

			if (_mergeSystem == null || _itemsFactory == null || _itemsDatabase == null) {
				D.Error($"{nameof(ItemMerger)} dependencies are not injected.");
				return;
			}

			if (!context.UserCharacter.TryGetComponent<IInventoryComponent>(
				out var inventoryComponent) || !inventoryComponent.HasItem)
				return;
			

			var item = inventoryComponent.CurrentItem;
			if (_supportedInputTypes != null && !_supportedInputTypes.Contains(item.ItemType)) {
				D.Log($"{nameof(ItemMerger)} cannot accept item type {item.ItemType}.");
				return;
			}

			if (_bufferedItems.Count >= _maxBufferedItems) {
				D.Log($"{nameof(ItemMerger)} buffer is full.");
				return;
			}

			var slotIndex = _bufferedItems.Count;
			var inputPosition = GetInputPositionForItem(slotIndex);
			if (inputPosition == null) {
				D.Log($"{nameof(ItemMerger)} has no input slot transform for index {slotIndex}.");
				return;
			}

			if (!inventoryComponent.TryTakeItem(out var itemVisual))
				return;

			_bufferedItems.Add(item);
			PlaceBufferedInputVisual(itemVisual, inputPosition);
			D.Log($"{nameof(ItemMerger)} added {item.Name}({item.ItemId}). Buffered: {_bufferedItems.Count}");
		}

		public bool TryStartMergeProcess() {
			if (_isProcessing) {
				D.Log($"{nameof(ItemMerger)} is already processing.");
				return false;
			}

			if (_bufferedItems.Count == 0) {
				D.Log($"{nameof(ItemMerger)} buffer is empty.");
				return false;
			}

			var inputIds = _bufferedItems.Select(x => x.ItemId).ToArray();

			if (!_mergeSystem.TryGetMergedProductId(inputIds, out var outputItemId)) {
				D.Log($"{nameof(ItemMerger)} cannot merge current buffer. Recipe was not found.");
				return false;
			}

			_processingInputInfo = BuildProductsInfo(_bufferedItems);
			_processingOutputItemId = outputItemId;
			PoolBufferedInputVisuals();
			_bufferedItems.Clear();

			_remainingTime = _processingTimeSeconds;
			StartProcessing();
			return true;
		}

		private void Update() {
			if (!_isProcessing) {
				return;
			}

			ItemProcessingSquashAnimation.Tick(ref _squashState, Time.deltaTime);

			_remainingTime -= Time.deltaTime;
			if (_remainingTime > 0f) {
				return;
			}

			CompleteProcessing();
		}

		private static string BuildProductsInfo(IReadOnlyList<Item> items) {
			var sb = new StringBuilder();
			for (var i = 0; i < items.Count; i++) {
				if (i > 0) {
					sb.Append(", ");
				}

				sb.Append(items[i].Name);
				sb.Append("(");
				sb.Append(items[i].ItemId);
				sb.Append(")");
			}

			return sb.ToString();
		}

		private Transform GetInputPositionForItem(int bufferedIndex) {
			if (_inputItemPositions == null
			|| bufferedIndex < 0
			|| bufferedIndex >= _inputItemPositions.Length)
				return null;

			return _inputItemPositions[bufferedIndex];
		}

		private void PlaceBufferedInputVisual(ItemVisual itemVisual, Transform inputPosition) {
			if (itemVisual == null || inputPosition == null)
				return;

			itemVisual.transform.SetParent(inputPosition, false);
			itemVisual.transform.localPosition = Vector3.zero;
			itemVisual.transform.localRotation = Quaternion.identity;
			itemVisual.SetInteractionColliderEnabled(false);
			itemVisual.gameObject.SetActive(true);
			_bufferedItemVisuals.Add(itemVisual);
		}

		private void PoolBufferedInputVisuals() {
			if (_itemsFactory == null)
				return;

			for (var i = 0; i < _bufferedItemVisuals.Count; i++) {
				var visual = _bufferedItemVisuals[i];
				if (visual != null)
					_itemsFactory.ReturnToPool(visual);
			}

			_bufferedItemVisuals.Clear();
		}
		
		protected virtual void StartProcessing() {
			_isProcessing = true;
			PlayProcessingSquash();
		}

		protected virtual void CompleteProcessing() {
			StopProcessingSquash();

			var outputPosition = _outputItemPosition != null ? _outputItemPosition.position : transform.position;
			var outputRotation = _outputItemPosition != null ? _outputItemPosition.rotation : transform.rotation;
			var itemVisual = _itemsFactory.Create(_processingOutputItemId, outputPosition, outputRotation);

			var outputName = _itemsDatabase.TryGetItemByItemId(_processingOutputItemId, out var outputItem)
				? outputItem.Name
				: "Unknown";

			D.Log($"{nameof(ItemMerger)} merged {_processingInputInfo} to {outputName}({_processingOutputItemId}) visual:{itemVisual.name}");
			_isProcessing = false;
		}

		private void PlayProcessingSquash() {
			var visual = _processingVisualTransform != null ? _processingVisualTransform : transform;
			ItemProcessingSquashAnimation.Start(
				ref _squashState,
				visual,
				_squashHalfCycleSeconds,
				_squashXZStretch,
				_squashYMul);
		}

		private void StopProcessingSquash() {
			ItemProcessingSquashAnimation.Stop(ref _squashState);
		}

		private void OnDisable() {
			StopProcessingSquash();
			PoolBufferedInputVisuals();
			_bufferedItems.Clear();
		}
	}
}
