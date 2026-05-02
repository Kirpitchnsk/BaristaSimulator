namespace SibGameJam2026.Characters.Components {
	public class HealthCharacterComponent : IHealthCharacterComponent {
		private readonly ACharacter _character;
		public ACharacter Character => _character;

		public int MaxHealth { get; }
		public int Health { get; private set; }

		public HealthCharacterComponent(ACharacter character, int maxHealth) {
			_character = character;
			MaxHealth = maxHealth;
			Health = maxHealth;
		}

		public void TakeDamage(int damage) {
			if (damage <= 0) {
				return;
			}

			Health -= damage;
			if (Health < 0) {
				Health = 0;
			}
		}

		public void Heal(int amount) {
			if (amount <= 0) {
				return;
			}

			Health += amount;
			if (Health > MaxHealth) {
				Health = MaxHealth;
			}
		}
	}
}
