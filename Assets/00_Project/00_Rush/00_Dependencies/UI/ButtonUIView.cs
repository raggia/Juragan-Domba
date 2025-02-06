using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rush
{
    [System.Serializable]
    public abstract class ButtonViewSetting
    {
        [SerializeField]
        private Sprite m_Frame;
        public Sprite Frame => m_Frame;
    }
    public abstract class ButtonUIView : UIView
    {
        [SerializeField]
        private Button m_Button;
        [SerializeField]
        protected Image m_Frame;
        [SerializeField]
        protected TextMeshProUGUI m_ButtonText;

        public void SetText(string val)
        {
            m_ButtonText.text = val;
        }
        public void AddAction(UnityAction act)
        {
            m_Button.onClick?.AddListener(act);
        }
        public void RemoveAction(UnityAction act)
        {
            m_Button.onClick?.RemoveListener(act);
        }
        public void ClearAction()
        {
            m_Button?.onClick?.RemoveAllListeners();
        }

        public void SetInteractableButton(bool interactable)
        {
            m_Button.interactable = interactable;
        }
        public void SetFrame(Sprite frame)
        {
            m_Frame.sprite = frame;
        }
    }
}
