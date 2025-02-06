using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class SFXClipField
    {
        [SerializeField]
        private AudioClip m_FootStepGrass;
        public AudioClip FootStepGrass => m_FootStepGrass;
    }
    public partial class AudioSingleton
    {
        [SerializeField]
        private SFXClipField m_SFXClipField;
        public void PlaySFX(AudioClip clip, Transform spawn)
        {
            Play<SFXHandler>(clip, spawn);
        }
        public void SetSFXVolume(float volume)
        {
            string mixGrup = GetHandler<SFXHandler>().MixGroup;
            SetVolume(mixGrup, volume);
        }
    }
    public partial class AudioAgent
    {
        
        public void PlaySFX(AudioClip clip)
        {
            AudioSingleton.Instance.PlaySFX(clip, transform);
        }
    }
    [System.Serializable]
    public partial class SFXHandler : AudioHandler
    {
        
    }
}
