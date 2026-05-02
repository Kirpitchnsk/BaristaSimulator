using System;
using UnityEngine;

namespace SibGameJam2026.Characters {
	[Serializable]
	public class CharacterData {
		[field: SerializeField] public Rigidbody Rigidbody { get; private set; }
		[field: SerializeField] public CharacterController CharacterController { get; private set; }
		[field: SerializeField] public Animator Animator { get; private set; }
		[field: SerializeField] public Transform CameraPoint { get; private set; }
		[field: SerializeField] public Transform ItemPosition { get; private set; }
	}
}
