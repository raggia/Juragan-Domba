using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class PlayerCart : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private CartState m_CartState = CartState.Buy;
        [SerializeField, ReadOnly]
        private ShopObject m_Shop;
        
        [SerializeField]
        private UnityEvent<ShopObject> m_OnSetShopForBuy = new();
        [SerializeField]
        private UnityEvent<ShopObject> m_OnSetShopForSell = new();

        [SerializeField]
        private UnityEvent<IItem> m_OnAddCart = new();
        [SerializeField]
        private UnityEvent<IItem> m_OnRemoveCart = new();
        [SerializeField]
        private UnityEvent<List<IItem>> m_OnClearCart = new();

        [SerializeField]
        private UnityEvent<IItem> m_OnPriceChanged = new();


        [SerializeField, ReadOnly]
        private Currency m_TotalPrice;
        public CartState CartState => m_CartState;
        public Inventory GetShopInventory()
        {
            return m_Shop.GetShopInventory();
        }
        private List<IItem> GetShopItemsInternal()
        {
            return m_Shop.GetShopItems();
        }
        public void AddToCart(IItem item)
        {
            m_Shop.AddToCart(item);
            Item i = m_Shop.GetItemCart(item.Id);
            OnAddCartInvoke(i);
        }

        private void SetCartState(CartState state)
        {
            m_CartState = state;
        }

        public void RemoveFromCart(IItem item)
        {
            
            Item i = m_Shop.GetItemCart(item.Id);
            OnRemoveCartInvoke(i);
            m_Shop.RemoveFromCart(i);
            
        }
        public void ClearCart()
        {
            OnClearCartInvoke(GetCartItems());

            m_Shop.ClearCart();
        }
        public void SetShopForBuy(ShopObject shop)
        {
            m_Shop = shop;
            OnSetShopForBuyInvoke(shop);
        }
        public void SetShopForSell(ShopObject shop)
        {
            m_Shop = shop;
            OnSetShopForSellInvoke(shop);
        }
        private void OnAddCartInvoke(IItem item)
        {
            m_OnAddCart?.Invoke(item);
            UpdatePrice();
        }
        private void OnRemoveCartInvoke(IItem item)
        {
            m_OnRemoveCart?.Invoke(item);
            UpdatePrice();
        }
        private void OnClearCartInvoke(List<IItem> items)
        {
            m_OnClearCart?.Invoke(items);
        }
        private void OnPriceChangeInvoke(Currency price)
        {
            m_OnPriceChanged?.Invoke(price);
        }

        private void SetPriceState(IItem item)
        {
            int amount = item.Amount;
            int price = 0;
            ItemDefinition defi = item.Definition;
            switch (m_CartState) 
            {
                case CartState.Buy:
                    price = m_Shop.GetFinalBuyPrice(item.Definition).Amount;
                    defi = item.GetBuyCurrencyDefinition();
                    break;
                case CartState.Sell:
                    price = m_Shop.GetFinalSellPrice(item.Definition).Amount;
                    defi = item.GetSellCurrencyDefinition();
                    break;
            }
            int final = price * amount;
            Item i = new (defi, "", final);
            AddPrice(i);
        }

        private void AddPrice(IItem add)
        {
            m_TotalPrice.SetDefinition(add.Definition);
            m_TotalPrice.AddAmount(add.Amount);
        }

        public void UpdatePrice()
        {
            m_TotalPrice.SetAmount(0);
            foreach (Item item in GetCartItems())
            {
                SetPriceState(item);
            }
            OnPriceChangeInvoke(m_TotalPrice);
        }

        public void Pay()
        {
            PlayerSingleton.Instance.PayCart(m_TotalPrice, GetCartItems());
        }
        
        private List<IItem> GetCartItems()
        {
            return m_Shop.GetCartInventory().GetItems();
        }
        public Inventory GetCartInventory()
        {
            return m_Shop.GetCartInventory();
        }

        private void OnSetShopForBuyInvoke(ShopObject shop)
        {
            m_OnSetShopForBuy?.Invoke(shop);
            SetCartState(CartState.Buy);
        }
        private void OnSetShopForSellInvoke(ShopObject shop)
        {
            m_OnSetShopForSell?.Invoke(shop);
            SetCartState(CartState.Sell);
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

    public partial class PlayerSingleton
    {
        [SerializeField]
        private PlayerCart m_TargetShop;
        public CartState CartState => m_TargetShop.CartState;
        public void AddToCart(IItem item)
        {
            m_TargetShop.AddToCart(item);
        }

        public void Pay()
        {
            m_TargetShop.Pay();
        }

        public void RemoveFromCart(IItem item)
        {
            m_TargetShop.RemoveFromCart(item);
        }
        public void SetShopForBuy(ShopObject shop)
        {
            m_TargetShop.SetShopForBuy(shop);
        }

        public void SetShopForSell(ShopObject shop)
        {
            m_TargetShop.SetShopForSell(shop);
        }

        public Currency GetFinalBuyPrice(ItemDefinition defi)
        {
            return m_TargetShop.GetFinalBuyPrice(defi);
        }
        public Currency GetFinalSellPrice(ItemDefinition defi)
        {
            return m_TargetShop.GetFinalSellPrice(defi);
        }

        public Inventory GetCartInventory()
        {
            return m_TargetShop.GetCartInventory();
        }
        public Inventory GetShopInventory()
        {
            return m_TargetShop.GetShopInventory();
        }
        public void ClearCart()
        {
            m_TargetShop.ClearCart();
        }
    }
}
