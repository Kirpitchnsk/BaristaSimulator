using System;
using UnityEngine;
using UnityEngine.Audio;


namespace Arenar.AudioSystem
{
    [Serializable]
    public class AudioMixerData
    {
        [SerializeField] private AudioMixerGroup audioMixerGroup;
        [SerializeField] private SerializableDictionary<AudioSystemType, string> audioGroupNames;


        public AudioMixerGroup AudioMixerGroup => audioMixerGroup;
        public SerializableDictionary<AudioSystemType, string> AudioGroupNames => audioGroupNames;
    }
}