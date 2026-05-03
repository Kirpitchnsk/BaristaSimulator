using System;
using UnityEngine;


namespace Arenar.AudioSystem
{
    [Serializable]
    public class UiSoundsLibrary
    {
        [SerializeField] private SerializableDictionary<UiSoundType, AudioClip> uiSounds = default;


        public SerializableDictionary<UiSoundType, AudioClip> UiSounds => uiSounds;
    }
}
