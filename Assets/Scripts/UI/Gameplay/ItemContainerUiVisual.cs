using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arenar.Services.UI {
	public class ItemContainerUiVisual : MonoBehaviour {
		[SerializeField] private Image _portrait;
		[SerializeField] private TMP_Text _itemName;
		
		public void SetItem(Sprite sprite, string itemName) {
			_portrait.sprite = sprite;
			_itemName.text = itemName;
		}
	}
}