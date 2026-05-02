namespace SibGameJam2026.Characters {
	public enum EClientState : byte {
		WalkToOrder = 0,
		WaitInteraction = 1,
		WaitCooking = 2,
		TransformCreatureSuccess = 3,
		TransformCreatureFailed = 4,
		NonTransformed = 5,
		Leave = 6,
		Finished = 7,
	}
}
