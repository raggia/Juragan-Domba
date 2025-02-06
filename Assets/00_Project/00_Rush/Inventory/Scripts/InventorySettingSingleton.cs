using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class InventorySettingSingleton : Singleton<InventorySettingSingleton>
    {
        [Header("Inventory Setting")]
        [SerializeField]
        private InventoryResources m_InventoryResources;

        public ItemDefinition GetItemDefinition(string id)
        {
            ItemDefinition match = m_InventoryResources.GetItemDefinition(id);
            return match;
        }
        public CurrencyDefinition GetCurrencyDefinition(string id)
        {
            return m_InventoryResources.GetCurrencyDefinition(id);
        }
        public Sprite GetCurrencyIcon(string id)
        {
            Sprite match = m_InventoryResources.GetStarterCurrencyIcon(id);
            return match;
        }
        public Sprite GetItemIcon(string id)
        {
            return m_InventoryResources.GetItemIcon(id);
        }
        public Sprite GetRaritySprite(string id)
        {
            return m_InventoryResources.GetItemRaritySprite(id);
        }
        public string GetLabel(string id)
        {
            return m_InventoryResources.GetItemLabel(id);
        }

        public T GetItemBehaviour<T>(string id) where T : ItemBehaviour
        {
            return m_InventoryResources.GetItemBehaviour<T>(id);
        }
        public bool IsUniqueItem(string id)
        {
            return m_InventoryResources.IsUniqueItem(id);
        }

        public string GetSlotId(string id)
        {
            return m_InventoryResources.GetItemSlotId(id);
        }
    }
}
