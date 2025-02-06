using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Rush
{

    [System.Serializable]
    public abstract partial class AudioHandler 
    {
        [SerializeField]
        private string m_ParameterName;
        [SerializeField]
        private AudioMixerGroup m_Group;
        [SerializeField]
        private bool m_IsSpawn = false;
        [SerializeField]
        private bool m_IsLoop = false;
        public bool IsSpawn => m_IsSpawn;
        public string MixGroup => m_Group.name;

        [SerializeField, ReadOnly]
        private List<AudioSource> m_AudioSources = new();

        private AudioSource GetAudiSource(int instanceId)
        {
            AudioSource match = m_AudioSources.Find(x => x.GetInstanceID() == instanceId);
            if (match == null)
            {
                Debug.Log($"There is no Audio Source that contain {instanceId} in {m_AudioSources}");
                match = null;
            }
            return match;
        }
        public virtual void Play(AssetReferenceGameObject asset, AudioClip clip, Transform spawn, bool world = false)
        {
            int random = Random.Range(-1000, 1000);
            CoroutineUtility.BeginCoroutine($"{random}/{asset.SubObjectName}/{clip.name}/{nameof(Playing)}", Playing(asset, clip, spawn, world));
        }
        protected virtual IEnumerator Playing(AssetReferenceGameObject asset, AudioClip clip, Transform spawn, bool world = false)
        {
            if (!AddressableUtility.HasPrefab(asset.AssetGUID))
            {
                 yield return AddressableUtility.LoadingPrefab(asset.AssetGUID);
            }
            AudioSource prefab = AddressableUtility.GetPrefab<AudioSource>(asset.AssetGUID);
            AudioSource audio = GameObject.Instantiate(prefab, spawn, world);

            audio.outputAudioMixerGroup = m_Group;
            audio.loop = m_IsLoop;
            audio.clip = clip;
            //audio.Stop();
            if (!HasAudioSource(audio.GetInstanceID()))
            {
                m_AudioSources.Add(audio);
            }
            audio.Play();

            if (!m_IsLoop)
            {
                yield return new WaitUntil(() => !audio.isPlaying);
                m_AudioSources.Remove(audio);
                GameObject.Destroy(audio.gameObject);
                Debug.Log($"Audio Handler Destroy {audio.clip}");
                //AddressableUtility.ReleaseInstance(mGetAudiSource(audio.GetInstanceID()));
            }
            
            // on stop
        }

        private bool HasAudioSource(int instanceId)
        {
            return GetAudiSource(instanceId) != null;
        }
        public virtual void Stop()
        {
            foreach (AudioSource audio in m_AudioSources)
            {
                audio.Stop();
            }
        }
    }
}
