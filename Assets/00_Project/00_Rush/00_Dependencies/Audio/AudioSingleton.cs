using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Rush
{

    public partial class AudioSingleton : Singleton<AudioSingleton>
    {
        [SerializeField]
        private AssetReferenceGameObject m_AudioSourceAsset;
        [SerializeField]
        private AudioSource m_AudioSourceInstance; // single source, BGM
        [SerializeField]
        private AudioMixer m_Mixer;
        [SerializeReference, SubclassSelector]
        private List<AudioHandler> m_AudioHandlers = new();

        private T GetHandler<T>() where T : AudioHandler
        {
            AudioHandler audioHandler = m_AudioHandlers.Find(x => x.GetType() == typeof(T));
            if (audioHandler == null)
            {
                audioHandler = null;
                Debug.LogWarning($"There is no Audiohandler Type {typeof(T)} exist on {m_AudioHandlers}");
            }
            return (T)audioHandler;
        }

        private void Play<T>(AudioClip clip, Transform spawn, bool world = false) where T : AudioHandler
        {
            GetHandler<T>().Play(m_AudioSourceAsset, clip, spawn, world);
        }

        private void SetVolume(string parameterName, float volume)
        {
            if (m_Mixer != null)
            {
                float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
                m_Mixer.SetFloat(parameterName, dbVolume);
            }
            else
            {
                Debug.LogWarning("AudioMixer is not assigned!");
            }
        }
        private float GetVolume(string parameterName)
        {
            if (m_Mixer != null)
            {
                if (m_Mixer.GetFloat(parameterName, out float dbVolume))
                {
                    return Mathf.Pow(10f, dbVolume / 20f);
                }
                else
                {
                    Debug.LogWarning($"Parameter '{parameterName}' not found in AudioMixer!");
                }
            }
            else
            {
                Debug.LogWarning("AudioMixer is not assigned!");
            }
            return 0f;
        }

        private void Stop<T>() where T : AudioHandler
        {
            GetHandler<T>().Stop();
        }
    }
}

