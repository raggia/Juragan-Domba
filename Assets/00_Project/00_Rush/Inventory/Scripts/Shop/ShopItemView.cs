using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rush
{
    public enum CartState
    {
        Buy = 0,
        Sell = 1,
    }
    public class ShopItemView : ItemView
    {
        [SerializeField]
        private TextMeshProUGUI m_ItemNameText;
        
        [SerializeField]
        private Button m_AddButton;
        [SerializeField]
        private Button m_RemoveButton;

        [SerializeField]
        private TextMeshProUGUI m_PriceText;
        [SerializeField]
        private Image m_CurrencyIcon;

        private Currency GetBuyCurrency(ItemDefinition defi)
        {
            return PlayerSingleton.Instance.GetFinalBuyPrice(defi);
        }
        private Currency GetSellCurrency(ItemDefinition defi)
        {
            return PlayerSingleton.Instance.GetFinalSellPrice(defi);
        }
        public void SetUpForShop(Item item)
        {
            SetUpInternal(item);
            int amount = item.Amount;
            int price = GetBuyCurrency(item.Definition).Amount;
            int finalPrice = price * amount;
            m_PriceText.text = finalPrice.ToString();
            m_ItemNameText.text = item.Label;
            m_CurrencyIcon.sprite = item.GetBuyCurrencyIcon();

            
        }

        protected override void SetUpInternal(IItem item)
        {
            base.SetUpInternal(item);
            int amount = item.Amount;
            int price = GetBuyCurrency(item.Definition).Amount;
            int finalPrice = price * amount;
            m_PriceText.text = finalPrice.ToString();
            m_ItemNameText.text = item.Label;
            m_CurrencyIcon.sprite = item.GetBuyCurrencyIcon();
            ShopStateButton();
        }
        public void SetUpForBuyCart(IItem item)
        {
            SetUpInternal(item);
            int amount = item.Amount;
            int price = GetBuyCurrency(item.Definition).Amount;
            int finalPrice = price * amount;
            m_PriceText.text = finalPrice.ToString();
            m_ItemNameText.text = item.Label;
            m_CurrencyIcon.sprite = item.GetBuyCurrencyIcon();

            CartStateButton();

            Debug.Log($"Set Up Cart For Buy {item.Label}");
        }

        public void SetUpForSellCart(IItem item)
        {
            SetUpInternal(item);
            int amount = item.Amount;
            int price = GetSellCurrency(item.Definition).Amount;
            int finalPrice = price * amount;
            m_PriceText.text = finalPrice.ToString();
            m_ItemNameText.text = item.Label;
            m_CurrencyIcon.sprite = item.GetSellCurrencyIcon();

            CartStateButton();
            Debug.Log($"Set Up Cart For Buy {item.Label}");
        }

        private void ShopStateButton()
        {
            m_RemoveButton.gameObject.SetActive(false);
            m_AddButton.gameObject.SetActive(true);
        }
        private void CartStateButton()
        {
            m_RemoveButton.gameObject.SetActive(true);
            m_AddButton.gameObject.SetActive(false);
        }

        public void AddActionOnShopButton(UnityAction action)
        {
            m_AddButton.onClick.AddListener(action);
        }
        public void RemovectionOnShopButton(UnityAction action)
        {
            m_AddButton.onClick.RemoveListener(action);
        }
        public void AddActionOnCartButton(UnityAction action)
        {
            m_RemoveButton.onClick.AddListener(action);
        }
        public void RemovectionOnCartButton(UnityAction action)
        {
            m_RemoveButton.onClick.RemoveListener(action);
        }
    }
}
