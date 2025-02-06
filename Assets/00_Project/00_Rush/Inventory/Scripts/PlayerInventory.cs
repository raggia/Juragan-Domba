using System.Collections.Generic;
using UnityEngine;

namespace Rush
{

    public partial class PlayerInventory : Inventory
    {
        
    }

    public partial class PlayerSingleton : IInventory
    {
        [SerializeField]
        private PlayerInventory m_Inventory;
        public PlayerInventory Inventory => m_Inventory;
        public List<Item> Items => m_Inventory.Items;

        public string Id => m_Inventory?.Id;
        public void SetItem(IItem item)
        {
            m_Inventory.SetItem(item);
        }
        public void PayCart(Currency other, List<IItem> carts)
        {
            m_Inventory.PayCart(other, carts);
        }
        public void AddItem(IItem item)
        {
            m_Inventory.AddItem(item);
        }

        public void Collects(List<IItem> items)
        {
            m_Inventory?.Collects(items);
        }

        public void RemoveItem(IItem item)
        {
            m_Inventory.RemoveItem(item);
        }

        public void SetCurrency(ICurrency currency)
        {
            m_Inventory.SetCurrencyAmount(currency);
        }
        public void AddCurrency(ICurrency currency)
        {
            m_Inventory.AddCurrencyAmount(currency);
        }
    }
}
