namespace SibGameJam2026.Services {
	public class PlayerInventoryService : IInventoryService {
		private int _coins = 0;
		
		public void Add(int count) {
			_coins += count;
		}

		public void Remove(int count) {
			_coins -= count;
			if (_coins <= 0) 
				_coins = 0;
		}
	}
}