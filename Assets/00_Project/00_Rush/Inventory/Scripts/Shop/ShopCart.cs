using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class ShopCart : Inventory
    {
        [SerializeField]
        private ShopDefinition m_ShopDefi;
        [SerializeField]
        private CartState m_CartState = CartState.Buy;

        public void SetState(CartState state)
        {
            m_CartState = state;
        }
        public void SetShopDefinition(ShopDefinition shopDefi)
        {
            m_ShopDefi = shopDefi;
        }
        protected override void OnItemAmountCollectedInvoke(IItem item)
        {
            base.OnItemAmountCollectedInvoke(item);

            UpdatePrices();
        }
        protected override void OnItemRemovedInvoke(IItem item)
        {
            base.OnItemRemovedInvoke(item);

            UpdatePrices();
        }
        private Currency GetFinalBuyPrice(Item item)
        {
            Currency currency = m_ShopDefi.GetFinalBuyPrice(item.Definition);

            return currency;
        }
        private Currency GetFinalSellPrice(Item item)
        {
            Currency currency = m_ShopDefi.GetFinalSellPrice(item.Definition);

            return currency;
        }
        private void UpdatePrices()
        {
            m_Currencies.Clear();
            foreach (Item item in m_Items)
            {
                switch(m_CartState)
                {
                    case CartState.Buy:
                        AddCurrencyAmountInternal(GetFinalBuyPrice(item));
                        break;
                    case CartState.Sell:
                        AddCurrencyAmountInternal(GetFinalSellPrice(item));
                        break;
                }
            }
        }
        public void OnNewAddToCartListen(UnityAction<IItem> action)
        {
            m_OnNewItemAdded?.AddListener(action);
        }
        public void OnAddToCartListen(UnityAction<IItem> action)
        {
            m_OnItemAmountChanged?.AddListener(action);
        }
        public void OnRemoveFromCartListen(UnityAction<IItem> action)
        {
            m_OnItemRemoved?.RemoveListener(action);
        }

        public void ClearCartEvents()
        {
            m_OnNewItemAdded?.RemoveAllListeners();
            m_OnItemAmountChanged?.RemoveAllListeners();
            m_OnItemRemoved?.RemoveAllListeners();
        }

        private bool CanBuy(List<Currency> payer)
        {

        }
    }

    public partial class ShopObject
    {
        [SerializeField]
        private ShopCart m_Cart;
        public void AddToCart(IItem item)
        {
            m_Cart.AddItem(item);
        }

        public void RemoveFromCart(IItem item)
        {
            m_Cart.RemoveItem(item);
        }
        public void OnNewAddToCartListen(UnityAction<IItem> action)
        {
            m_Cart.OnNewAddToCartListen(action);
        }
        public void OnAddToCartListen(UnityAction<IItem> action)
        {
            m_Cart.OnNewAddToCartListen(action);
        }
        public void OnRemoveFromCartListen(UnityAction<IItem> action)
        {
            m_Cart.OnNewAddToCartListen(action);
        }

        public void ClearCartEvents()
        {
            m_Cart.ClearCartEvents();
        }

        public Inventory GetCartInventory()
        {
            return m_Cart;
        }

        public Item GetItemCart(string id)
        {
            return m_Cart.GetItem(id); 
        }

        public void ClearCart()
        {
            m_Cart.ClearItems();
        }
    }
}
