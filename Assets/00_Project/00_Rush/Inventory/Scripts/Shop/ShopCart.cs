using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class ShopCart : Inventory
    {
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
