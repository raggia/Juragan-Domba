using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Rush
{
    public partial class ShopPanel : PanelView
    {
        [SerializeField, ReadOnly]
        private ShopObject m_Shop;
        [SerializeField]
        private ShopView m_ShopView;
        [SerializeField]
        private InventoryUIView m_PlayerInventoryView;
        [SerializeField]
        private CartView m_CartView;

        [SerializeField]
        private TextMeshProUGUI m_TotalPriceText;

        

        public void ShowToBuy(ShopObject shop)
        {
            m_Shop = shop;
            ShowInternal();
            m_ShopView.Show();
            m_PlayerInventoryView.Hide();
        }

        public void ShowToSell()
        {
            ShowInternal();
            m_ShopView.Hide();
            m_PlayerInventoryView.Show();
        }
    }

    [System.Serializable]
    public partial class ShopObjectChair : EventSingleChairField<ShopObject>
    {

    }

    public partial class NewEventTicketDefinition
    {
        public void Execute(ShopObject shop)
        {
            ActionHandler<ShopObject>.ExecuteAction(name, shop);
        }
    }

    public partial class NewEventBus
    {
        public void RegisterShopObject()
        {
            RegisterSingleChair<ShopObject>();
        }
        public void UnRegisterShopObject()
        {
            UnRegisterSingleChair<ShopObject>();
        }
    }
}
