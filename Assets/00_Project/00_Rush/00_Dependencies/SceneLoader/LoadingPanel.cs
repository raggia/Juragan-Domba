using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rush
{
    public partial class LoadingPanel : PanelView
    {
        [SerializeField]
        private TextMeshProUGUI m_LoadingText;

        [SerializeField]
        private Slider m_LoadingProgress;
        
        public void SetLoadingProgresValue(float progressRate)
        {
            m_LoadingProgress.value = progressRate;
        }
        public void SetLoadingProgressText(string text)
        {
            m_LoadingText.text = text;
        }
    }
}
