using TMPro;
using UnityEngine;

namespace Arenar.Services.UI {
	public class GameplayMarkCanvasWindowLayer : CanvasWindowLayer {
		[SerializeField] private TMP_Text _itemNameText;

		public void SetVisible(bool isVisible) {
			gameObject.SetActive(isVisible);
		}

		public void SetItemName(string itemName) {
			if (_itemNameText != null)
				_itemNameText.text = itemName;
		}
	}
}
