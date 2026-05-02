namespace SibGameJam2026.Services {
    public interface IGameService {
        bool IsGameActive { get; }

        void StartGame();
        void CompleteGame();
    }
}