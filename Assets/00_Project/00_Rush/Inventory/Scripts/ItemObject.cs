using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial interface IItemCollectable 
    {
        IItem Collect();
    }
    public partial interface IItemObject : IItemCollectable, IItem, IHasDefinition<ItemDefinition>
    {
        
    }
    public class ItemObject : Interactable, IItemObject
    {
        [SerializeField]
        private ItemDefinition m_Definition;
        [SerializeField]
        private int m_Amount;
        [SerializeField, ReadOnly]
        private string m_SlotId;

        [SerializeField]
        private UnityEvent<IItem> m_OnCollected = new();

        protected override string IdInternal => base.IdInternal + m_Definition.Id + GetSlotIdInternal();

        public int Amount => m_Amount;

        public ItemDefinition Definition => m_Definition;

        public string Label => m_Definition.Label;

        public string ItemId => m_Definition.Id;

        public string SlotId => GetSlotIdInternal();

        public bool IsUniqueItem => m_Definition.IsUniqueItem();

        public string Description => m_Definition.Description;

        public Sprite GetIcon()
        {
            return InventorySettingSingleton.Instance.GetItemIcon(m_Definition.Id);
        }
        public Sprite GetRaritySprite()
        {
            return InventorySettingSingleton.Instance.GetRaritySprite(m_Definition.Id);
        }

        public void AddAmount(int amount)
        {
            m_Amount += amount;
        }
        
        public void SetAmount(int amount)
        {
            m_Amount = amount;
        }

        public void SetDefinition(ItemDefinition val)
        {
            m_Definition = val;
        }
        public override string GetInteractText()
        {
            return base.GetInteractText() + $" {m_Definition.Label} x{m_Amount}";
        }

        public IItem Collect()
        {
            IItem item = new Item(m_Definition, GetSlotIdInternal(), m_Amount);
            if (m_Definition.IsUniqueItem())
            {
                item.SetAmount(1);
            }
            OnCollectedInvoke(item);
            return item;
        }
        private void OnCollectedInvoke(IItem item)
        {
            m_OnCollected?.Invoke(item);
            SetBusyInternal(false);
            TryDeContactInternal();
        }

        public T GetBehaviour<T>() where T : ItemBehaviour
        {
            return m_Definition.GetItemBehaviour<T>();
        }

        private string GetSlotIdInternal()
        {
            if (string.IsNullOrEmpty(m_SlotId))
            {
                m_SlotId = m_Definition.GetSlotId();
            }
            return m_SlotId;
        }

        public Sprite GetBuyCurrencyIcon()
        {
            return m_Definition.GetBuyCurrencyIcon();
        }
        public Sprite GetSellCurrencyIcon()
        {
            return m_Definition.GetSellCurrencyIcon();
        }

        public ItemDefinition GetBuyCurrencyDefinition()
        {
            return m_Definition.GetBuyCurrencyDefinition();
        }

        public ItemDefinition GetSellCurrencyDefinition()
        {
            return m_Definition.GetSellCurrencyDefinition();
        }
    }
}
