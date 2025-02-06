using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public enum EInitTriggerOn
    {
        None = 0,
        Awake = 1,
        Enable = 2,
        Start = 3,
    }
    public interface IInitializer<T>
    {
        void Init(T val = default);
        bool Initialized { get; }
    }
    public partial class Initiator : MonoBehaviour
    {
        [SerializeField]
        private string m_Id = string.Empty;
        [SerializeField]
        private bool m_BusyInitialization = false;
        [SerializeField]
        private EInitTriggerOn m_TriggerOn = EInitTriggerOn.None;

        [SerializeField, ReadOnly]
        private bool m_IsInitialzed = false;
        [SerializeField]
        private UnityEvent<object> m_OnInitDone = new();
        public bool IsInitialized => m_IsInitialzed;
        protected virtual void Awake()
        {
            if (m_TriggerOn == EInitTriggerOn.Awake)
            {
                InitInternal();
            }
        }
        protected virtual void OnEnable()
        {
            if (m_TriggerOn == EInitTriggerOn.Enable)
            {
                InitInternal();
            }
        }

        protected virtual void Start()
        {
            if (m_TriggerOn == EInitTriggerOn.Start)
            {
                InitInternal();
            }
        }
        public virtual void Init(object val = null)
        {
            InitInternal(val);
        }
        protected virtual void InitInternal(object val = null)
        {
            if (m_IsInitialzed) return;
            CoroutineUtility.BeginCoroutine(m_Id, Initing(val), m_BusyInitialization);
        }
        protected virtual IEnumerator Initing(object val)
        {
            Debug.Log($"{m_Id} is initialized");
            yield return new WaitForEndOfFrame();
            OnInitDoneInvoked(val);
        }

        protected virtual void OnInitDoneInvoked(object val) 
        {
            //Debug.Log($"{m_Id} is initialized");
            m_OnInitDone?.Invoke(val);
            m_IsInitialzed = true;
        }
    }
}

