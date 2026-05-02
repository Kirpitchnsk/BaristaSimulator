using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class D {
	[Conditional("UNITY_EDITOR")]
	public static void Log(object message) {
		Debug.Log(message);
	}

	[Conditional("UNITY_EDITOR")]
	public static void Warning(object message) {
		Debug.LogWarning(message);
	}

	[Conditional("UNITY_EDITOR")]
	public static void Error(object message) {
		Debug.LogError(message);
	}
}

