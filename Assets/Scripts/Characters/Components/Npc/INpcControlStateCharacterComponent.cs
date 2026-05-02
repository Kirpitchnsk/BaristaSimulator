using System;
using SibGameJam2026.Characters;

namespace SibGameJam2026.Characters.Components {
	public interface INpcControlStateCharacterComponent : ICharacterComponent {
		EClientState State { get; }

		event Action<EClientState> StateChanged;

		void SetState(EClientState next);
	}
}
