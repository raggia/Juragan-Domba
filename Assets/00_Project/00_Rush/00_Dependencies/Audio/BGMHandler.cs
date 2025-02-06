using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class BGMField
    {
        [SerializeField]
        private AudioClip m_HomeBGM;
        [SerializeField]
        private AudioClip m_CharCreationBGM;
        public AudioClip HomeBGM => m_HomeBGM;
        public AudioClip CharCreationBGM => m_CharCreationBGM;
    }
    public partial class AudioSingleton
    {
        [SerializeField]
        private BGMField m_BGMField;
        public void PlayHomeBGM()
        {
            //CoroutineUtility.BeginCoroutine($"{name}/{m_BGMField.HomeBGM}", Playing<BGMHandler>(m_BGMField.HomeBGM, transform));
            Play<BGMHandler>(m_BGMField.HomeBGM, transform);
        }
        public void PlayCharacterCreationBGM()
        {
            //CoroutineUtility.BeginCoroutine($"{name}/{m_BGMField.CharCreationBGM}", Playing<BGMHandler>(m_BGMField.CharCreationBGM, transform));
            Play<BGMHandler>(m_BGMField.CharCreationBGM, transform);
        }
        public void PlayBGM(AudioClip clip)
        {
            Play<BGMHandler>(clip, transform);
        }
        public void SetBGMVolume(float volume)
        {
            string parameterName = GetHandler<UIAudioHandler>().MixGroup;
            SetVolume(parameterName, volume);
        }
        public void StopBGM()
        {
            Stop<BGMHandler>();
        }
    }
    [System.Serializable]
    public partial class BGMHandler : AudioHandler
    {

    }
}
