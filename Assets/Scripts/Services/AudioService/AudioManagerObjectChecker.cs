using System;
using UnityEngine;


namespace Arenar.AudioSystem
{
    public class AudioManagerObjectChecker : MonoBehaviour
    {
        public Action<bool> OnApplicationPauseAction;
        
        
        private void OnApplicationPause(bool pauseStatus) =>
            OnApplicationPauseAction?.Invoke(pauseStatus);
    }
}