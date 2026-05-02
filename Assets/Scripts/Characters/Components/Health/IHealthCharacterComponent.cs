namespace SibGameJam2026.Characters.Components {
	public interface IHealthCharacterComponent : ICharacterComponent {
		int MaxHealth { get; }
		int Health { get; }

		void TakeDamage(int damage);
		void Heal(int amount);
	}
}
