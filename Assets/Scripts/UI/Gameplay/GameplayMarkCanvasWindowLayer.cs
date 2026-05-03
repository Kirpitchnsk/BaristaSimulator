using TMPro;
using UnityEngine;

namespace Arenar.Services.UI {
	public class GameplayMarkCanvasWindowLayer : CanvasWindowLayer {
		[SerializeField] private TMP_Text _itemNameText;

		public void SetVisible(bool isVisible)
			=> gameObject.SetActive(isVisible);

		public void SetItemName(string itemName) {
			if (_itemNameText == null) 
				return;
			_itemNameText.text = itemName;
			var euler = _itemNameText.rectTransform.localEulerAngles;
			euler.z = Random.Range(-15f, 15f);
			_itemNameText.rectTransform.localEulerAngles = euler;
		}
	}
}
