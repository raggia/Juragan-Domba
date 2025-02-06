using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rush
{
    public partial class ItemView : ButtonUIView, IInitializer<IItem>
    {
        [SerializeField]
        private Image m_Icon;
        [SerializeField, ReadOnly]
        private bool m_Initialized;
        public bool Initialized => m_Initialized;

        [SerializeField, ReadOnly]
        private Item m_Item;

        [SerializeField]
        private UnityEvent<IItem> m_OnInitialized = new();

        public virtual void Init(IItem val = null)
        {
            if (m_Initialized) return;
            SetUpInternal(val);
            CoroutineUtility.BeginCoroutine($"{m_Id}_{nameof(Initializing)}", Initializing(val));
        }

        protected virtual void SetUpInternal(IItem item)
        {
            m_Id = item.Id;
            m_Item = new Item(item.Definition, item.SlotId, item.Amount);
            m_Icon.sprite = item.GetIcon();
            m_Frame.sprite = item.GetRaritySprite();
            m_ButtonText.text = item.Amount.ToString();
        }

        public virtual void SetUp(IItem item)
        {
            SetUpInternal(item);
        }

        protected virtual IEnumerator Initializing(IItem val)
        {
            yield return new WaitForEndOfFrame();
            OnInitializedInvoked(val);
        }

        protected virtual void OnInitializedInvoked(IItem val)
        {
            m_Initialized = true;
            m_OnInitialized?.Invoke(val);
        }
        public IItem Item => GetItemInternal();
        private IItem GetItemInternal()
        {
            return m_Item;
        }
    }
}
