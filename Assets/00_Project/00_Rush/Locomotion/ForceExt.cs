using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public abstract class ForceExt<T> where T : Locomotion
    { 
        [SerializeField]
        protected string m_ForceName;

        [SerializeField, ReadOnly]
        protected T m_Locomotion;
        [SerializeField, ReadOnly]
        protected Vector3 m_Force;

        [SerializeField, ReadOnly]
        protected bool m_Initialized = false;
        public string ForceName => m_ForceName;
        public Vector3 Force => m_Force;
        public bool Initialized => m_Initialized;

        [SerializeField]
        private UnityEvent<T> m_OnInitialized = new();
        public void Initialize(T set)
        {
            if (m_Initialized) return;
            CoroutineUtility.BeginCoroutine(m_ForceName, Initializing(set));
        }

        public IEnumerator Initializing(T set)
        {
            yield return new WaitForEndOfFrame();
            m_Locomotion = set;
            OnInitializedInvoked(set);
        }

        protected void OnInitializedInvoked(T set)
        {
            m_OnInitialized?.Invoke(set);
            m_Initialized = true;
        }

        public void ApplyForce()
        {
            if (!m_Initialized) return;
            OnApplyForceInvoked();
        }
        protected virtual void OnApplyForceInvoked()
        {
            
        }
    }

    [System.Serializable]
    public abstract class CCForceExt : ForceExt<CCLocoMotion>
    {

    }
}
