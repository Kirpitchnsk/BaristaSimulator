using System;
using UnityEngine;

namespace Arenar.Options
{
    [Serializable]
    public class UiSoundOption : IOption
    {
        private const float MAX_VOLUME = 1f;
        
        
        public bool isActive;
        public float volume;


        public UiSoundOption()
        {
            this.isActive = true;
            this.volume = MAX_VOLUME;
        }
        

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        
        public float Volume
        {
            get => volume;
            set => volume = Mathf.Clamp01(value);
        }
    }
}
