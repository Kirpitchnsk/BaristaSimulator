using SibGameJam2026.Characters;
using SibGameJam2026.Items;
using SibGameJam2026.Settings;

namespace SibGameJam2026.Services {
	public interface ILevelService {
		/// <summary>Активные настройки уровня; null, если уровень не начат.</summary>
		GameSettingsData ActiveLevel { get; }

		int CookFailureCount { get; }

		/// <summary>Индекс текущего клиента в <see cref="SibGameJam2026.Settings.GameSettingsData.ClientData"/> (-1, если никого не заспавнили).</summary>
		int CurrentActiveClientIndex { get; }

		/// <param name="levelKey">Ключ <see cref="SibGameJam2026.Settings.GameSettingsData.Key"/>; null или пусто — первый элемент массива настроек.</param>
		void BeginLevel(string levelKey = null);
		void EndLevel();

		void AssignCookingDishForClient(ACharacter clientNpc);

		bool TryGetExpectedDish(ACharacter clientNpc, out ItemId dishId);

		void PresentClientOrderUi(ACharacter clientNpc);
		void StartActiveClientCookingTimer();

		/// <summary>Активный клиент успешно получил нужный напиток — учёт успеха и индикация в UI.</summary>
		void SetCookSuccess();

		/// <summary>Истёк таймер готовки у активного клиента — учёт провала и индикация в UI.</summary>
		void SetCookFailed();

		/// <summary>FSM текущего клиента из очереди; false, если активного NPC нет.</summary>
		bool TryGetActiveClientState(out EClientState state);
	}
}
