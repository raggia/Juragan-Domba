using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rush
{
    public partial class StatView : UIView
    {
        [SerializeField]
        protected Stat m_Stat;
        [SerializeField]
        protected Image m_Icon;
        [SerializeField]
        protected TextMeshProUGUI m_ValueText;

        public StatNameDefinition Definition => m_Stat.Definition;
        public virtual void SetUp(Stat stat)
        {
            ShowInternal();
            m_Stat = stat;
            m_Icon.sprite = m_Stat.Definition.Icon;
            m_ValueText.text = m_Stat.Value.ToString();
        }
    }
}
