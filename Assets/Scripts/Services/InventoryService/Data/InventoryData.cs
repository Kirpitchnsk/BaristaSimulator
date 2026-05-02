using UnityEngine;

namespace SibGameJam2026.Services {
	[CreateAssetMenu(fileName = "InventoryData", menuName = "Data/InventoryData", order = 51)]
	public class InventoryData : ScriptableObject
	{
		public int CellsCount = 16;
	}
}