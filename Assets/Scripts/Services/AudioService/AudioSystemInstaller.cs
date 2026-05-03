using UnityEngine;
using Zenject;


namespace  Arenar.AudioSystem
{
    [CreateAssetMenu(menuName = "Audio System/Audio system settings")]
    public class AudioSystemInstaller : ScriptableObjectInstaller<AudioSystemInstaller>
    {
        [SerializeField] private AudioMixerData audioMixerData = default;



    }
}
