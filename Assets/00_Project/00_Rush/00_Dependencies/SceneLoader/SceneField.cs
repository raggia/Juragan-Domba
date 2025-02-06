using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Rush
{
    [System.Serializable]
    public class SceneField
    {
        [SerializeField, ReadOnly]
        private float m_Progress;
        [SerializeField, ReadOnly]
        private string m_ProgressText;
        [SerializeField]
        private string m_SceneName;
        [SerializeField]
        private LoadSceneMode m_LoadSceneMode;

        [SerializeField]
        private UnityEvent m_OnLoadSceneStart = new();
        [SerializeField]
        private UnityEvent m_OnLoadSceneDone = new();

        private AsyncOperation m_Handle;
        public string SceneName => m_SceneName;
        public LoadSceneMode LoadSceneMode => m_LoadSceneMode;

        public string Id => m_SceneName;

        public Coroutine LoadAsync()
        {
            return CoroutineUtility.BeginCoroutineReturn($"{m_SceneName}/{nameof(LoadingAsync)}", LoadingAsync());
        }
        public Coroutine UnLoadAsync()
        {
            return CoroutineUtility.BeginCoroutineReturn($"{m_SceneName}/{nameof(UnLoadingAsync)}", UnLoadingAsync());
        }

        private IEnumerator LoadingAsync()
        {
            OnLoadSceneStartInvoke();
            SetProgressValue(0f);
            m_Handle = SceneManager.LoadSceneAsync(m_SceneName, m_LoadSceneMode);
            string progressText = "";
            while (!m_Handle.isDone)
            {
                SetProgressValue(m_Handle.progress);
                progressText = $"Loading {m_SceneName} Scene in Progress {m_Progress * 100f:F1}%";
                SetLoadText(progressText);
                yield return null;
            }
            yield return new WaitUntil(() => m_Handle.isDone);

            m_Handle.allowSceneActivation = false;
            SetProgressValue(1f);
            progressText = $"Loading {m_SceneName} Scene in Progress {m_Progress * 100f:F1}%";
            SetLoadText(progressText);

            yield return new WaitForSeconds(0.5f);
            SetLoadText($"Loading {m_SceneName} Scene is Completed");
            m_Handle.allowSceneActivation = true;
            yield return new WaitForSeconds(0.5f);
            OnLoadSceneDoneInvoke();
        }
        private IEnumerator UnLoadingAsync()
        {
            SetProgressValue(0f);
            string unloadText = $"Wait... UnLoading {m_SceneName} Scene in Progress";
            SetLoadText(unloadText);
            yield return SceneManager.UnloadSceneAsync(m_SceneName);

            SetLoadText($"UnLoading {m_SceneName} Scene is Completed");
            yield return new WaitForSeconds(0.5f);
        }
        private void SetLoadText(string set)
        {
            m_ProgressText = set;
            SceneLoaderSingleton.Instance.SetLoadText(set);
        }
        private void SetProgressValue(float set)
        {
            m_Progress = set;
            SceneLoaderSingleton.Instance.SetLoadProgressValue(set);
        }
        private void OnLoadSceneStartInvoke()
        {
            m_OnLoadSceneStart?.Invoke();
        }
        private void OnLoadSceneDoneInvoke()
        {
            m_OnLoadSceneDone?.Invoke();
        }
    }
}
