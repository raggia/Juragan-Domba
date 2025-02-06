using UnityEngine;
using UnityEngine.UI;

namespace Rush
{
    public partial class HomePanel : PanelView
    {
        [SerializeField]
        private string m_StartSceneName;
        [SerializeField]
        private Button m_StartButton;

        protected override void Start()
        {
            base.Start();
            ShowInternal(0.3f);
            m_StartButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            SceneLoaderSingleton.Instance.LoadScene(m_StartSceneName);
            HideInternal();
        }
    }
}
