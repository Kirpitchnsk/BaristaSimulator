using SibGameJam2026.Items;
using SibGameJam2026.MergeService;
using SibGameJam2026.Characters.Components;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SibGameJam2026 {
	public class ItemTransformer : AInteractItemVisual {
		[SerializeField] private List<EItemType> _supportedInputTypes;
		[SerializeField] private float _processingTimeSeconds = 5f;
		[SerializeField] private Transform _inputItemPosition;
		[SerializeField] private Transform _outputItemPosition;

		[Header("Processing visual (Teapot, etc.)")]
		[SerializeField] private Transform _processingVisualTransform;
		[SerializeField] private float _squashHalfCycleSeconds = 0.35f;
		[SerializeField] private float _squashXZStretch = 1.06f;
		[SerializeField] private float _squashYMul = 0.88f;

		private bool _isProcessing;
		private float _remainingTime;
		private Item _processingInputItem;
		private ItemId _processingOutputItemId;
		private IMergeSystem _mergeSystem;
		private ItemsFactory _itemsFactory;
		private ItemsDatabase _itemsDatabase;
		private ItemProcessingSquashState _squashState;

		[Inject]
		private void Construct(IMergeSystem mergeSystem, ItemsFactory itemsFactory, ItemsDatabase itemsDatabase) {
			_mergeSystem = mergeSystem;
			_itemsFactory = itemsFactory;
			_itemsDatabase = itemsDatabase;
		}

		public override void OnInteract(InteractContext context) {
			if (_mergeSystem == null || _itemsFactory == null || _itemsDatabase == null) {
				D.Error($"{nameof(ItemTransformer)} dependencies are not injected.");
				return;
			}
			
			if (_isProcessing) {
				D.Log($"{nameof(ItemTransformer)} is busy.");
				return;
			}

			if (!context.UserCharacter.TryGetComponent<IInventoryComponent>(
				out var inventoryComponent) || !inventoryComponent.HasItem)
				return;

			var item = inventoryComponent.CurrentItem;
			if (_supportedInputTypes == null || !_supportedInputTypes.Contains(item.ItemType)) {
				D.Log($"{nameof(ItemTransformer)} cannot process item type {item.ItemType}.");
				return;
			}
			
			if (!_mergeSystem.TryGetMergedProductId(new[] { item.ItemId }, out var outputItemId)) {
				D.Log($"{nameof(ItemTransformer)} cannot transform item {item.Name}({item.ItemId}). Recipe was not found.");
				return;
			}

			if (!inventoryComponent.TryTakeItem(out var itemVisual))
				return;

			_processingOutputItemId = outputItemId;
			_processingInputItem = item;
			_remainingTime = _processingTimeSeconds;
			ProcessConsumedItemVisual(itemVisual, _inputItemPosition);
			
			StartProcessing();
		}

		private void Update() {
			if (!_isProcessing)
				return;

			ItemProcessingSquashAnimation.Tick(ref _squashState, Time.deltaTime);

			_remainingTime -= Time.deltaTime;
			if (_remainingTime > 0f)
				return;

			CompleteProcessing();
		}

		protected virtual void StartProcessing() {
			_isProcessing = true;
			PlayProcessingSquash();
		}

		private void ProcessConsumedItemVisual(ItemVisual itemVisual, Transform inputPosition) {
			if (itemVisual == null)
				return;

			if (inputPosition != null) {
				itemVisual.transform.SetPositionAndRotation(inputPosition.position, inputPosition.rotation);
				itemVisual.SetInteractionColliderEnabled(false);
			}

			_itemsFactory.ReturnToPool(itemVisual);
		}

		protected virtual void CompleteProcessing() {
			StopProcessingSquash();

			var outputPosition = _outputItemPosition != null ? _outputItemPosition.position : transform.position;
			var outputRotation = _outputItemPosition != null ? _outputItemPosition.rotation : transform.rotation;
			var outputVisual = _itemsFactory.Create(_processingOutputItemId, outputPosition, outputRotation);

			var outputName = _itemsDatabase.TryGetItemByItemId(_processingOutputItemId, out var outputItem)
				? outputItem.Name
				: "Unknown";

			D.Log($"{nameof(ItemTransformer)} processed {_processingInputItem.Name}({_processingInputItem.ItemId}) to {outputName}({_processingOutputItemId}) visual:{outputVisual.name}");
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
		}
	}
}
