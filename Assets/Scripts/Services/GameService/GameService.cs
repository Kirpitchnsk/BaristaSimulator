using SibGameJam2026.Cameras;
using SibGameJam2026.Characters;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Services {
	public class GameService : IGameService, ITickable {
		private IInventoryService _inventoryService;
		private readonly ACharacter.Factory _characterFactory;
		private readonly ICameraService _cameraService;
		
		public GameService(
			IInventoryService inventoryService,
			ACharacter.Factory characterFactory,
			ICameraService cameraService) {
			_inventoryService = inventoryService;
			_characterFactory = characterFactory;
			_cameraService = cameraService;
			Debug.Log("GameService is Initialized" + inventoryService);
		}
		
		public bool IsGameActive { get; private set; } = false;

		public void Tick() {
		}
		
		public void StartGame() {
			if (IsGameActive)
				return;
			
			var character = _characterFactory.Create(ECharacterType.Player, Vector3.zero);
			var cameraParent = character.Data.CameraPoint != null ? character.Data.CameraPoint : character.transform;
			_cameraService.CreateCamera(ECameraType.FirstPerson, cameraParent);

			_inventoryService.Add(1);
			IsGameActive = true;
		}

		public void CompleteGame()
		{
			IsGameActive = false;
		}
	}
}