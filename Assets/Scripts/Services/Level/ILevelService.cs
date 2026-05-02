using SibGameJam2026.Characters;
using SibGameJam2026.Items;

namespace SibGameJam2026.Services {
	public interface ILevelService {
		int CookFailureCount { get; }

		/// <summary>Индекс текущего клиента в <see cref="SibGameJam2026.Settings.GameSettingsData.ClientData"/> (-1, если никого не заспавнили).</summary>
		int CurrentActiveClientIndex { get; }

		/// <param name="levelKey">Ключ <see cref="SibGameJam2026.Settings.GameSettingsData.Key"/>; null или пусто — первый элемент массива настроек.</param>
		void BeginLevel(string levelKey = null);

		void AssignCookingDishForClient(ACharacter clientNpc);

		bool TryGetExpectedDish(ACharacter clientNpc, out ItemId dishId);

		void PresentClientOrderUi(ACharacter clientNpc);

		/// <summary>Истёк таймер готовки у активного клиента — учёт провала и индикация в UI.</summary>
		void SetCookFailed();
	}
}
