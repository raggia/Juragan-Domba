using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rush
{
    public partial class CharacterPanel : PanelView
    {
        [SerializeField]
        private TextMeshProUGUI m_HeaderPageText;
        [SerializeField]
        private List<UIView> m_CharacterPages = new();

        [SerializeField, ReadOnly]
        private UIView m_CurrentPageShow;

        private UIView GetCurrentPageShow()
        {
            if (m_CurrentPageShow == null)
            {
                m_CurrentPageShow = m_CharacterPages[0];
            }
            return m_CurrentPageShow;
        }

        public override void Show(float overideDelay = 0)
        {
            base.Show(overideDelay);
            OpenPage();
        }
        public override void Hide(float delay = 0)
        {
            base.Hide(delay);
            foreach (var page in m_CharacterPages)
            {
                page.Hide();
            }
        }

        public void ChangeCharacterPage(string id)
        {
            UIView match = m_CharacterPages.Find(x => x.Id == id);
            if (match == null)
            {
                return;
            }
            m_CurrentPageShow = match;
            OpenPage();
        }

        private void OpenPage()
        {
            foreach (var page in m_CharacterPages)
            {
                if (page == GetCurrentPageShow())
                {
                    page.Show();
                }
                else
                {
                    page.Hide();
                }
                
            }
            m_HeaderPageText.text = GetCurrentPageShow().Id;

        }
    }
}
