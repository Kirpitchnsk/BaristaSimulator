using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Cameras {
	[CreateAssetMenu(fileName = "CamerasDatabase", menuName = "Data/CamerasDatabase")]
	public class CamerasDatabase : ScriptableObjectInstaller<CamerasDatabase> {
		[SerializeField] private List<CameraEntry> _entries = new();

		public override void InstallBindings() {
			Container.Bind<CamerasDatabase>().FromInstance(this).AsSingle();
		}

		public CameraEntry GetEntry(ECameraType eCameraType) {
			var entry = _entries.Find(x => x.ECameraType == eCameraType);
			if (entry == null) {
				throw new ArgumentOutOfRangeException(nameof(eCameraType), eCameraType, "Camera entry not found");
			}

			return entry;
		}
	}
}
