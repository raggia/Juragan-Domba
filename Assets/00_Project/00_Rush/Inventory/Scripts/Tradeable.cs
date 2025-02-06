using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class Tradeable : ItemBehaviour
    {
        [SerializeField]
        private Currency m_BuyCost;
        [SerializeField]
        private Currency m_SellCost;

        public ItemDefinition GetBuyDefinition()
        {
            return m_BuyCost.Definition;
        }
        public ItemDefinition GetSellDefinition()
        {
            return m_SellCost.Definition;
        }
        public Currency GetBuyCost()
        {
            return m_BuyCost;
        }
        public Currency GetSellCost()
        {
            return m_SellCost;
        }
        public Sprite GetBuyCurrencyIcon()
        {
            return m_BuyCost.GetIcon();
        }
        public Sprite GetSellCurrencyIcon()
        {
            return m_SellCost.GetIcon();
        }
    }

    public partial class Item
    {
        private Currency GetBuyCostInternal()
        {
            Tradeable tradeable = GetBehaviourInternal<Tradeable>();
            return tradeable.GetBuyCost();
        }
        private Currency GetSellCostInternal()
        {
            Tradeable tradeable = GetBehaviourInternal<Tradeable>();
            return tradeable.GetSellCost();
        }
        public ItemDefinition GetBuyCurrencyDefinition()
        {
            Tradeable tradeable = GetBehaviourInternal<Tradeable>();
            return tradeable.GetBuyDefinition();
        }
        public ItemDefinition GetSellCurrencyDefinition()
        {
            Tradeable tradeable = GetBehaviourInternal<Tradeable>();
            return tradeable.GetSellDefinition();
        }

        public int GetBuyCurrencyAmount()
        {
            return GetBuyCostInternal().Amount;
        }
        public int GetSellCurrencyAmount()
        {
            return GetSellCostInternal().Amount;
        }
        public Sprite GetBuyCurrencyIcon()
        {
            return GetBuyCostInternal().GetIcon();
        }
        public Sprite GetSellCurrencyIcon()
        {
            return GetSellCostInternal().GetIcon();
        }
    }

    public partial interface IItem
    {
        Sprite GetBuyCurrencyIcon();
        Sprite GetSellCurrencyIcon();
        ItemDefinition GetBuyCurrencyDefinition();
        ItemDefinition GetSellCurrencyDefinition();
    }

    public partial interface ICurrency
    {
        int GetBuyCurrencyAmount();
        int GetSellCurrencyAmount();
    }

    public partial class ItemDefinition
    {
        public Sprite GetBuyCurrencyIcon()
        {
            return GetItemBehaviour<Tradeable>().GetBuyCurrencyIcon();
        }
        public Sprite GetSellCurrencyIcon()
        {
            return GetItemBehaviour<Tradeable>().GetSellCurrencyIcon();
        }
        public ItemDefinition GetBuyCurrencyDefinition()
        {
            return GetItemBehaviour<Tradeable>().GetBuyDefinition();
        }
        public ItemDefinition GetSellCurrencyDefinition()
        {
            return GetItemBehaviour<Tradeable>().GetSellDefinition();
        }
    }
}
