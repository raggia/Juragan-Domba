using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rush
{
    public partial class Initialization : MonoBehaviour
    {
        [SerializeField]
        private string m_NextSceneName;
        [SerializeField, ReadOnly]
        private bool m_Initialized = false;
        public bool Initialized => m_Initialized;

        private void Start()
        {
            Init();
        }
        protected void Init()
        {
            if (m_Initialized) return;

            CoroutineUtility.BeginCoroutine($"{GetInstanceID()}/{nameof(Initializing)}", Initializing());
        }

        protected virtual IEnumerator Initializing()
        {
            yield return new WaitForEndOfFrame();
            SceneLoaderSingleton.Instance.LoadScene(m_NextSceneName);
            m_Initialized = true;

        }
    }
}
