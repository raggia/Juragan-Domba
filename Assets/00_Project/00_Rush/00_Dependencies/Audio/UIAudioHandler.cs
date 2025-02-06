using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class UIAudioField
    {
        [SerializeField]
        private AudioClip m_ButtonClick;
        public AudioClip ButtonClick => m_ButtonClick;
    }
    public partial class AudioSingleton
    {
        [SerializeField]
        private UIAudioField m_UIAudioField;
        public void PlayButtonClick()
        {
            Play<UIAudioHandler>(m_UIAudioField.ButtonClick, transform);
        }

        public void SetUIAudioVolume(float volume)
        {
            string parameterName = GetHandler<UIAudioHandler>().MixGroup;
            SetVolume(parameterName, volume);
        }
    }
    [System.Serializable]
    public partial class UIAudioHandler : AudioHandler
    {
    }
}
