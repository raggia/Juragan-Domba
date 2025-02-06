using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rush
{
    [System.Serializable]
    public class Tab
    {
        [SerializeField]
        private Button m_Button;
        [SerializeField]
        private UIView m_PageView;

        public void Init(UnityAction<Button> action)
        {
            m_Button.onClick?.AddListener(() => action(m_Button));
        }
        public Button GetButton() 
        { 
            return m_Button; 
        }

        public void Open()
        {
            m_PageView.Show();
            m_Button.interactable = false;
        }

        public void Close()
        {
            m_PageView.Hide();
            m_Button.interactable = true;
        }
    }
    public class TabHandler : MonoBehaviour
    {
        [SerializeField]
        private Tab m_MainTab;
        [SerializeField]
        private List<Tab> m_Tabs = new();

        private bool m_IsInited = false;

        [SerializeField]
        private UnityEvent m_OnTabClick;
        private void OnEnable()
        {
            //Init();
            //OpenTabCloseOthersInternal(m_MainTab.GetButton());
        }
        public void Init()
        {
            if (m_IsInited) {return;}
            foreach (Tab tab in m_Tabs)
            {
                tab.Init(OpenTabCloseOthersInternal);
            }
            //OpenTabCloseOthersInternal(m_MainTab.GetButton());
            m_IsInited = true;
        }

        private Tab GetTab(Button button)
        {
            Tab tab = m_Tabs.Find(x => x.GetButton() == button);
            return tab;
        }


        private void OpenTabCloseOthersInternal(Button button)
        {
            foreach (Tab tab in m_Tabs)
            {
                tab.Close();
            }
            GetTab(button).Open();
            OnTabClickInvoked();
        }

        private void OnTabClickInvoked()
        {
            m_OnTabClick?.Invoke();
        }
    }
}

