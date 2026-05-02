using UnityEngine;
using SibGameJam2026.Services;
using SibGameJam2026.Characters;
using Zenject;

namespace SibGameJam2026.Cameras {
	public class CameraController : MonoBehaviour {
		[SerializeField] private Vector3 _localOffset = Vector3.zero;
		[SerializeField] private float _lookSensitivity = 120f;
		[SerializeField] private float _bodyFollowYawSpeed = 360f;
		[SerializeField] private float _lookDeadZone = 0.1f;
		[SerializeField] private float _minPitch = -80f;
		[SerializeField] private float _maxPitch = 80f;
		[SerializeField] private string _lookVectorAction = "CameraRotation";

		private IInputService _inputService;
		private float _pitch;
		private float _yaw;

		public Transform Parent { get; private set; }
		public Transform YawTarget { get; private set; }

		public void Initialize(Transform parent, IInputService inputService) {
			Parent = parent;
			var character = parent.GetComponentInParent<ACharacter>();
			YawTarget = character != null ? character.transform : parent;
			_inputService = inputService;
			_yaw = YawTarget != null ? YawTarget.eulerAngles.y : parent.eulerAngles.y;
			_pitch = transform.eulerAngles.x;
		}

		private void LateUpdate() {
			if (Parent == null || _inputService == null) {
				return;
			}

			var lookInput = _inputService.GetVector(_lookVectorAction);

			if (Mathf.Abs(lookInput.x) > _lookDeadZone) {
				_yaw += lookInput.x * _lookSensitivity * Time.deltaTime;
			}

			if (Mathf.Abs(lookInput.y) > _lookDeadZone) {
				_pitch = Mathf.Clamp(_pitch - lookInput.y * _lookSensitivity * Time.deltaTime, _minPitch, _maxPitch);
			}

			if (YawTarget != null) {
				var targetBodyRotation = Quaternion.Euler(0f, _yaw, 0f);
				YawTarget.rotation = Quaternion.RotateTowards(
					YawTarget.rotation,
					targetBodyRotation,
					_bodyFollowYawSpeed * Time.deltaTime);
			}

			transform.position = Parent.position + Parent.TransformDirection(_localOffset);
			transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
		}


		public class Factory : PlaceholderFactory<ECameraType, Transform, CameraController> { }
	}
}
