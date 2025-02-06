using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class ShopItem
    {
        [SerializeField]
        private ItemDefinition m_Definition;
        [SerializeField]
        private int m_Amount;
        public string GetSloId()
        {
            return m_Definition.GetSlotId();
        }
        public ItemDefinition Definition => m_Definition;
        public int Amount => m_Amount;
        public ShopItem(ItemDefinition defi, int amount)
        {
            m_Definition = defi;
            m_Amount = amount;
        }
    }
    [CreateAssetMenu(fileName = "New Shop", menuName = "Rush/Inventory/Shop")]
    public partial class ShopDefinition : UnitDefinition
    {
        [SerializeField]
        private List<ShopItem> m_Items = new();
        [SerializeField]
        private List<Currency> m_AdditionalBuyPrice = new();
        [SerializeField]
        private List<CurrencyRate> m_BuyPriceRate = new();
        [SerializeField]
        private List<Currency> m_AdditionalSellPrice = new();
        [SerializeField]
        private List<CurrencyRate> m_SellPriceRate = new();

        [SerializeField, ReadOnly]
        private Currency m_FinalBuyPrice;
        [SerializeField, ReadOnly]
        private Currency m_FinalSellPrice;
        public List<Currency> AdditionalBuyPrice => m_AdditionalBuyPrice;
        public List<CurrencyRate> BuyPriceRates => m_BuyPriceRate;
        public List<Currency> AdditionalSellPrice => m_AdditionalSellPrice;
        public List<CurrencyRate> SellPriceRates => m_SellPriceRate;
        private ShopItem GetShopItemInternal(ItemDefinition item)
        {
            ShopItem match = m_Items.Find(x => x.Definition == item);
            return match;
        }
        private int GetShopItemAmount(ItemDefinition item)
        {
            return GetShopItemInternal(item).Amount;
        }
        public List<ShopItem> GetItems()
        {
            return m_Items;
        }
        public List<IItem> GetIItems()
        {
            List<IItem> items = new();
            foreach (var item in m_Items)
            {
                items.Add(new Item(item.Definition, item.GetSloId(), item.Amount));
            }
            return items;
        }
        private Tradeable GetTradeable(ItemDefinition defi)
        {
            return GetShopItemInternal(defi).Definition.GetItemBehaviour<Tradeable>();
        }

        private Currency GetCurrencyAdditionalBuyPrice(ItemDefinition defi)
        {
            Currency match = m_AdditionalBuyPrice.Find(x => x.Definition == defi);
            if (match == null)
            {
                match = new Currency(defi, "", 0);
            }
            return match;
        }

        private CurrencyRate GetCurrencyBuyPriceRate(ItemDefinition defi)
        {
            CurrencyRate match = m_BuyPriceRate.Find(x => x.Definition == defi);
            if (match == null)
            {
                match = new CurrencyRate(defi, 1f);
            }
            return match;
        }
        private Currency GetCurrencyAdditionalSellPrice(ItemDefinition defi)
        {
            Currency match = m_AdditionalSellPrice.Find(x => x.Definition == defi);
            if (match == null)
            {
                match = new Currency(defi, "", 0);
            }
            return match;
        }

        private CurrencyRate GetCurrencySellPriceRate(ItemDefinition defi)
        {
            CurrencyRate match = m_SellPriceRate.Find(x => x.Definition == defi);
            if (match == null)
            {
                match = new CurrencyRate(defi, 1f);
            }
            return match;
        }

        private Currency GetFinalBuyPricesInternal(ItemDefinition defi)
        {
            Currency buyCost = GetTradeable(defi).GetBuyCost();

            int amount = buyCost.Amount;
            int additional = GetCurrencyAdditionalBuyPrice(buyCost.Definition).Amount;
            float rate = GetCurrencyBuyPriceRate(buyCost.Definition).Rate;

            int finalDiscount = Mathf.RoundToInt(amount * rate);
            int finalBuyPrice = finalDiscount + additional;
            Currency cur = new Currency(buyCost.Definition, buyCost.SlotId, finalBuyPrice);
            m_FinalBuyPrice = cur;
            return m_FinalBuyPrice;
        }
        private Currency GetFinalSellPricesInternal(ItemDefinition defi)
        {
            Currency sellCost = GetTradeable(defi).GetSellCost();

            int amount = sellCost.Amount;
            int additional = GetCurrencyAdditionalSellPrice(sellCost.Definition).Amount;
            float rate = GetCurrencySellPriceRate(sellCost.Definition).Rate;

            int finalDiscount = Mathf.RoundToInt(amount * rate);
            int finalSellPrice = finalDiscount + additional;
            Currency cur = new Currency(sellCost.Definition, sellCost.SlotId, finalSellPrice);
            m_FinalSellPrice = cur;
            return m_FinalSellPrice;
        }
        public Currency GetFinalBuyPrice(ItemDefinition defi)
        {
            return GetFinalBuyPricesInternal(defi);
        }
        public Currency GetFinalSellPrice(ItemDefinition defi)
        {
            return GetFinalSellPricesInternal(defi);    
        }
    }
}
