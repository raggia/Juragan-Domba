using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial interface IView
    {
        void Show(float delay = 0f);
        void Hide(float delay = 0f);
        void ShowThenHideByDuration(float showDelay, float hideDelay, float showDuration);
        bool IsShowing { get; }
    }
    // used for hide of show gameobject
    [Serializable]
    public partial class ViewField
    {
        [SerializeField] 
        private GameObject m_Content;
        [SerializeField] 
        private float m_ShowDelay;
        [SerializeField] 
        private float m_HideDelay;
        [SerializeField]
        private UnityEvent m_OnShowStart = new();
        [SerializeField] 
        private UnityEvent m_OnHideStart = new();
        [SerializeField] 
        private UnityEvent m_OnShowDone = new ();
        [SerializeField] 
        private UnityEvent m_OnHideDone = new();

        public GameObject Content => m_Content;

        [SerializeField, ReadOnly]
        private bool m_IsShowing = false;
        public bool IsShowing => m_IsShowing;
        public void Init()
        {
            bool show = m_Content.activeSelf;
            m_IsShowing = show;
        }
        public IEnumerator Showing(float delay = 0f)
        {
            OnShowStartInvoked();
            float targetDelay = 0f;
            if (delay > 0.01f)
            {
                targetDelay = delay;
            }
            else
            {
                targetDelay = m_ShowDelay;
            }
            yield return new WaitForSeconds(targetDelay);
            m_IsShowing = true;
            m_Content.SetActive(m_IsShowing);
            OnShowDoneInvoked();
        }
        public IEnumerator Hiding(float delay = 0f)
        {
            OnHideStartInvoked();
            float targetDelay = 0f;
            if (delay > 0.01f)
            {
                targetDelay = delay;
            }
            else
            {
                targetDelay = m_HideDelay;
            }
            yield return new WaitForSeconds(targetDelay);
            m_IsShowing = false;
            m_Content.SetActive(m_IsShowing);
            OnHideDoneInvoked();
        }
        private void OnShowStartInvoked()
        {
            m_OnShowStart?.Invoke();
        }
        private void OnShowDoneInvoked()
        {
            m_OnShowDone?.Invoke();
        }
        private void OnHideStartInvoked()
        {
            m_OnHideStart?.Invoke();
        }
        private void OnHideDoneInvoked()
        {
            m_OnHideDone?.Invoke();
        }
    }
    public partial class View : MonoBehaviour, IView
    {
        [SerializeField] 
        private ViewField m_ViewField;
        protected virtual void Start()
        {
            m_ViewField.Init();
        }

        protected GameObject m_Content => m_ViewField.Content;
        public bool IsShowing => m_ViewField.IsShowing;

        [ContextMenu("Show", false)]
        public virtual void Show(float overideDelay = 0f)
        {
            ShowInternal(overideDelay);
        }
        [ContextMenu("Hide", false)]
        public virtual void Hide(float overideDelay = 0f)
        {
            HideInternal(overideDelay);
        }
        public virtual void ShowThenHideByDuration(float showDuration, float showDelay = 0f, float hideDelay = 0f)
        {
            ShowThenHideByDurationInternal(showDuration, showDelay, hideDelay);
        }
        public virtual void ShowThenHideByDuration(float showDuration)
        {
            ShowThenHideByDurationInternal(showDuration);
        }
        protected virtual void ShowInternal(float overideDelay = 0f)
        {
            if (m_ViewField.IsShowing)return;
            CoroutineUtility.BeginCoroutine($"{name}{GetInstanceID()}/{nameof(Showing)}", Showing(overideDelay));
        }
        protected virtual void HideInternal(float overideDelay = 0f)
        {
            if (!m_ViewField.IsShowing) return;
            CoroutineUtility.BeginCoroutine($"{name}{GetInstanceID()}/{nameof(Hiding)}", Hiding(overideDelay));
        }
        protected virtual void ShowThenHideByDurationInternal(float showDuration, float showDelay = 0f, float hideDelay = 0f)
        {
            CoroutineUtility.BeginCoroutine($"{name}{GetInstanceID()}/{nameof(ShowingThenHidingByDuration)}", ShowingThenHidingByDuration(showDuration, showDelay, hideDelay));
        }
        protected virtual IEnumerator Showing(float delay = 0f)
        {
            return m_ViewField.Showing(delay);
        }
        protected virtual IEnumerator Hiding(float delay = 0f)
        {
            return m_ViewField.Hiding(delay);
        }
        protected virtual IEnumerator ShowingThenHidingByDuration(float showDuration, float showDelay = 0f, float hideDelay = 0f)
        {
            ShowInternal(showDelay);
            yield return new WaitForSeconds(showDuration);
            HideInternal(hideDelay);
        }

        public virtual void Toggle(float delay = 0f)
        {
            bool isShow = !m_ViewField.IsShowing;
            if (isShow)
            {
                HideInternal(delay);
            }
            else
            {
                ShowInternal(delay);
            }
        }
    }
}