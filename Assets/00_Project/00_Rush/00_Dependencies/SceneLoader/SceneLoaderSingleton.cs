using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Rush
{
    public class SceneLoaderSingleton : Singleton<SceneLoaderSingleton>
    {
        [SerializeField, ReadOnly]
        private float m_Progress = 0f;
        [SerializeField, ReadOnly]
        private string m_LoadProgressText;

        [SerializeField]
        private List<SceneField> m_SceneFields = new();

        [SerializeField]
        private UnityEvent<string> m_OnLoadingProgressTextChange = new();
        [SerializeField]
        private UnityEvent<float> m_OnLoadingProgressValueChange = new();
        [SerializeField]
        private UnityEvent m_OnLoadSceneStart = new();
        [SerializeField]
        private UnityEvent m_OnLoadSceneDone = new();
        private SceneField GetSceneField(string assetId)
        {
            SceneField sceneField = m_SceneFields.Find(x => x.Id == assetId);
            if (sceneField == null)
            {
                Debug.LogError($"No Scene Asset {assetId} in fields");
                sceneField = null;
            }
            return sceneField;
        }
        public void LoadScene(string assetId)
        {
            CoroutineUtility.BeginCoroutine($"{gameObject.name}_{nameof(LoadingScene)}", LoadingScene(assetId));
        }
        private IEnumerator LoadingScene(string assetId)
        {
            OnLoadSceneStartInvoke(assetId);
            yield return new WaitForSeconds(0.5f);
            yield return GetSceneField(assetId).LoadAsync();
            //yield return CoroutineUtility.BeginCoroutineReturn($"{gameObject.name}_{nameof(TrackLoadingScene)}", TrackLoadingScene(assetId));
            OnLoadSceneDoneInvoke(assetId);
        }
        private IEnumerator UnloadingScenes()
        {
            for (int i = 0; i < m_SceneFields.Count; i++)
            {
                yield return m_SceneFields[i].UnLoadAsync();
            }
        }

        public void SetLoadText(string set)
        {
            m_LoadProgressText = set;
            OnLoadingProgressTextInvoke(m_LoadProgressText);

        }
        public void SetLoadProgressValue(float val)
        {
            m_Progress = val;
            OnLoadingProgressValueInvoke(m_Progress);
        }
        private void OnLoadingProgressTextInvoke(string progress)
        {
            m_OnLoadingProgressTextChange?.Invoke(progress);
        }
        private void OnLoadingProgressValueInvoke(float progress)
        {
            m_OnLoadingProgressValueChange?.Invoke(progress);
        }
        private void OnLoadSceneStartInvoke(string assetId)
        {
            m_OnLoadSceneStart?.Invoke();
        }
        private void OnLoadSceneDoneInvoke(string assetId)
        {
            m_OnLoadSceneDone?.Invoke();
        }
    }
}
