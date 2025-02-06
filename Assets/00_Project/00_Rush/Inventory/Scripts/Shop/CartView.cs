using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rush
{
    public partial class CartView : InventoryUIView
    {
        [SerializeField]
        private Image m_PriceCurrencyIcon;
        [SerializeField]
        private TextMeshProUGUI m_TotalPriceText;
        [SerializeField]
        private Button m_BuyButton;
        [SerializeField]
        private NotEnoughView m_NotEnoughToView;
        public override Inventory GetInventory()
        {
            return PlayerSingleton.Instance.GetCartInventory();
        }
        // on event ticket
        public void SetTotalPriceView(IItem set)
        {
            m_PriceCurrencyIcon.sprite = set.GetIcon();
            m_TotalPriceText.text = set.Amount.ToString();
        }
        protected override void ShowInternal(float overideDelay = 0)
        {
            base.ShowInternal(overideDelay);
            m_TotalPriceText.text = "0";
        }
        protected override void HideInternal(float overideDelay = 0)
        {
            base.HideInternal(overideDelay);
            ClearItemView();
            PlayerSingleton.Instance.ClearCart();
        }

        public void OpenNotEnoughtView()
        {
            m_NotEnoughToView.Show();
        }

        private void RemoveCart(IItem item)
        {
            //Debug.Log($"Remove Item {item.Id} from list {gameObject.name}");
            PlayerSingleton.Instance.RemoveFromCart(item);
        }

        protected override void OnItemViewAdded(IItem item)
        {
            base.OnItemViewAdded(item);

            SetUpForCart(item);
        }

        private void SetUpForCart(IItem item)
        {
            ShopItemView view = (ShopItemView)GetItemView(item.Id);
            switch (PlayerSingleton.Instance.CartState)
            {
                case CartState.Buy:
                    view.SetUpForBuyCart(item);
                    break;
                case CartState.Sell:
                    view.SetUpForSellCart(item);
                    break;
            }
            view.AddActionOnCartButton(() => RemoveCart(item));
        }
    }
}
