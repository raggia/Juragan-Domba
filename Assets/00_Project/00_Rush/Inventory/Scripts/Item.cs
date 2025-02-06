using UnityEngine;

namespace Rush
{
    public partial interface IItem : IIdentifier
    {
        int Amount { get; }
        void AddAmount(int amount);
        void SetAmount(int amount);
        Sprite GetIcon();
        Sprite GetRaritySprite();
        T GetBehaviour<T>() where T : ItemBehaviour;
        string Label { get; }
        string ItemId { get; }
        string SlotId { get; }
        string Description { get; }
        bool IsUniqueItem { get; }
        ItemDefinition Definition { get; }
        void SetDefinition(ItemDefinition defi);
        
    }
    [System.Serializable]
    public partial class Item : IItem
    {
        [SerializeField]
        private ItemDefinition m_Definition;
        [SerializeField, ReadOnly]
        private string m_SlotId;
        [SerializeField]
        private int m_Amount;
        private InventorySettingSingleton m_InventorySingleton;
        public string ItemId => m_Definition.Id;
        public int Amount => m_Amount;
        public string Label => GetLabel();
        public string Id => $"{m_Definition.Id}{GetSlotIdInternal()}";

        public string SlotId => GetSlotIdInternal();
        public ItemDefinition Definition => m_Definition;
        private string GetSlotIdInternal()
        {
            if (string.IsNullOrEmpty(m_SlotId))
            {
                m_SlotId = GetInventorySingleton().GetSlotId(m_Definition.Id);
            }
            if (IsUniqueItemInternal)
            {
                m_Amount = 1;
            }
            return m_SlotId;
        }
        private string GetLabel()
        {
            return GetInventorySingleton().GetLabel(m_Definition.Id);
        }
        public Sprite GetIcon()
        {
            return GetInventorySingleton().GetItemIcon(m_Definition.Id);
        }
        public Sprite GetRaritySprite()
        {
            return GetInventorySingleton().GetRaritySprite(m_Definition.Id);
        }
        public Item(ItemDefinition definition, string slotId, int amount)
        {
            m_Definition = definition;
            m_SlotId = slotId;
            m_Amount = amount;
        }
        public Item(IItem item)
        {
            m_Definition = item.Definition;
            m_SlotId = item.SlotId;
            m_Amount = item.Amount;
        }

        public virtual void AddAmount(int amount)
        {
            m_Amount += amount;
            ClampAmount();
        }
        public virtual void SetAmount(int amount)
        {
            m_Amount = amount;
            ClampAmount();
        }
        public T GetBehaviour<T>() where T : ItemBehaviour
        {
            return GetBehaviourInternal<T>();
        }
        public T GetBehaviourInternal<T>() where T : ItemBehaviour
        {
            T behaviour = GetInventorySingleton().GetItemBehaviour<T>(m_Definition.Id);
            return behaviour;
        }

        private void ClampAmount()
        {
            m_Amount = Mathf.Clamp(m_Amount, 0, int.MaxValue);
        }
        public bool IsUniqueItem => IsUniqueItemInternal;
        public bool IsUniqueItemInternal => GetInventorySingleton().IsUniqueItem(m_Definition.Id);

        public string Description => m_Definition.Description;

        private InventorySettingSingleton GetInventorySingleton() 
        {
            if (m_InventorySingleton == null)
            {
                m_InventorySingleton = InventorySettingSingleton.Instance;
            }
            return m_InventorySingleton;  
        }

        public void SetDefinition(ItemDefinition defi)
        {
            m_Definition = defi;
        }
    }
}
