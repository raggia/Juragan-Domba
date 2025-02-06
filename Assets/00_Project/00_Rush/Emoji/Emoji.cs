using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class Emoji : View
    {
        [SerializeField]
        private SpriteRenderer m_SpriteRenderer;

        public void ShowEmote(Sprite emote)
        {
            if (m_SpriteRenderer == null) return;
            m_SpriteRenderer.sprite = emote;
            ShowInternal();
            Debug.Log($"Show Emoji {emote.name}");
        }

        public void ShowEmoteOnce(Sprite emote)
        {
            if (m_SpriteRenderer == null) return;
            m_SpriteRenderer.sprite = emote;
            ShowThenHideByDurationInternal(5f);
            Debug.Log($"Show Emoji {emote.name}");
        }
    }

    [System.Serializable]
    public partial class SpriteChair : EventSingleChairField<Sprite>
    {

    }

    public partial class NewEventTicketDefinition
    {
        public void Execute(Sprite sprite)
        {
            ActionHandler<Sprite>.ExecuteAction(name, sprite);
        }
    }
    public partial class NewEventBus
    {
        public void RegisterSpriteChair()
        {
            RegisterSingleChair<Sprite>();
        }
        public void UnRegisterSpriteChair()
        {
            UnRegisterSingleChair<Sprite>();
        }
    }
}
