namespace SibGameJam2026.Services {
	public interface IGameService {
		bool IsGameActive { get; }

		/// <param name="levelKey">Ключ уровня (<see cref="SibGameJam2026.Settings.GameSettingsData.Key"/>); null — уровень по умолчанию в <see cref="ILevelService"/>.</param>
		void StartGame(string levelKey = null);

		void CompleteGame();
	}
}
