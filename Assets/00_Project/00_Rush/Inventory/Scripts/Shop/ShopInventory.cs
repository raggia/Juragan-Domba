using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class ShopInventory : Inventory
    {
        [SerializeField]
        private ShopDefinition m_ShopDefinition;

        private void Start()
        {
            CollectsInternal(m_ShopDefinition.GetIItems());
        }
        public Currency GetFinalBuyPrice(ItemDefinition defi)
        {
            return m_ShopDefinition.GetFinalBuyPrice(defi);
        }
        public Currency GetFinalSellPrice(ItemDefinition defi)
        {
            return m_ShopDefinition.GetFinalSellPrice(defi);
        }
    }

    public partial class ShopObject
    {
        [SerializeField]
        private ShopInventory m_Shop;

        public Inventory GetShopInventory()
        {
            return m_Shop;
        }

        public List<IItem> GetShopItems()
        {
            return m_Shop.GetItems();
        }
        public Currency GetFinalBuyPrice(ItemDefinition defi)
        {
            return m_Shop.GetFinalBuyPrice(defi);
        }
        public Currency GetFinalSellPrice(ItemDefinition defi)
        {
            return m_Shop.GetFinalSellPrice(defi);
        }
    }
}
