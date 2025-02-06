using UnityEngine;

namespace Rush
{
    [CreateAssetMenu(fileName = "New Item Resources", menuName = "Rush/Inventory/Item Resources")]
    public class InventoryResources : UnitDefinition
    {
        [SerializeField]
        private ItemDefinition[] m_RegisteredItemDefinitions;

        [SerializeField]
        private CurrencyDefinition[] m_StarterCurrencies;

        [SerializeField]
        private ItemDefinition m_UnRegisteredItemDefinition; // diisi dengan item kosong
        [SerializeField]
        private CurrencyDefinition m_UnRegisteredCurrencyDefinition; // diisi dengan currency kosong
        private ItemDefinition GetItemDefinitionInternal(string itemId)
        {
            ItemDefinition match = m_UnRegisteredItemDefinition;
            foreach (var item in m_RegisteredItemDefinitions)
            {
                if (item.Id == itemId)
                {
                    match = item;
                    return match;
                } 

            }
            return match;
        }
        private CurrencyDefinition GetCurrencyDefinitionInternal(string currencyId)
        {
            CurrencyDefinition match = m_UnRegisteredCurrencyDefinition;
            foreach (var currency in m_StarterCurrencies)
            {
                if (currency.Id == currencyId)
                {
                    match = currency;
                    return match;
                }

            }
            return match;
        }
        public Sprite GetStarterCurrencyIcon(string itemId)
        {
            return GetCurrencyDefinitionInternal(itemId).Icon;
        }
        public bool IsUniqueItem(string itemId)
        {
            return GetItemDefinitionInternal(itemId).IsUniqueItem();
        }
        public ItemDefinition GetItemDefinition(string itemId)
        {
            return GetItemDefinitionInternal(itemId);
        }
        public CurrencyDefinition GetCurrencyDefinition(string currencyId)
        {
            return GetCurrencyDefinitionInternal(currencyId);
        }
        public Sprite GetItemIcon(string itemId)
        {
            return GetItemDefinitionInternal(itemId).Icon;
        }
        public Sprite GetItemRaritySprite(string itemId)
        {
            return GetItemDefinitionInternal(itemId).GetRaritySprite();
        }

        public string GetItemLabel(string itemId)
        {
            return GetItemDefinitionInternal(itemId).Label;
        }

        public string GetItemSlotId(string itemId)
        {
            return GetItemDefinitionInternal(itemId).GetSlotId();
        }

        public T GetItemBehaviour<T>(string itemId) where T : ItemBehaviour
        {
            return GetItemDefinitionInternal(itemId).GetItemBehaviour<T>();
        }
    }
}
