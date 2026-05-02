using SibGameJam2026.Cameras;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Services {
	public class GameService : IGameService, ITickable {
		private IInventoryService _inventoryService;
		private readonly ILevelService _levelService;
		private readonly IInputService _inputService;
		private readonly ICameraService _cameraService;

		public GameService(
			IInventoryService inventoryService,
			ILevelService levelService,
			IInputService inputService,
			ICameraService cameraService
		) {
			_inventoryService = inventoryService;
			_levelService = levelService;
			_inputService = inputService;
			_cameraService = cameraService;
			Debug.Log("GameService is Initialized" + inventoryService);
		}

		public bool IsGameActive { get; private set; } = false;

		public void Tick() {
		}

		public void StartGame(string levelKey = null) {
			if (IsGameActive)
				return;

			_cameraService.EnsureGameplayCamera();
			_levelService.BeginLevel(levelKey);

			_inventoryService.Add(1);
			_inputService.SwitchActionMap("PlayerInputMap");
			IsGameActive = true;
		}

		public void CompleteGame() {
			IsGameActive = false;
		}
	}
}
