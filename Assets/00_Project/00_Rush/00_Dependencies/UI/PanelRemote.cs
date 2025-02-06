using UnityEngine;

namespace Rush
{
    public partial class PanelRemote : MonoBehaviour
    {
        [SerializeField]
        private string m_PanelId;

        private PanelView m_Panel;

        private PanelView GetPanelInternal()
        {
            if (m_Panel == null)
            {
                m_Panel = CanvasSingleton.Instance.GetPanel(m_PanelId);
            }
            return m_Panel;
        }
        public PanelView GetPanel() => GetPanelInternal();

        public virtual void ShowPanel()
        {
            GetPanelInternal().Show();
        }
    }
}
