using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class CurrenciesMonitorView : UIView
    {
        [SerializeField]
        private List<CurrencyView> m_CurrencyViews = new();
        [SerializeField, ReadOnly]
        private Inventory m_Inventory;
        private CurrencyView GetCurrencyView(string currencyId)
        {
            CurrencyDefinition currencyDefinition = InventorySettingSingleton.Instance.GetCurrencyDefinition(currencyId);
            CurrencyView currencyView = m_CurrencyViews.Find(x => x.Definition == currencyDefinition);
            if (currencyView == null)
            {
                Debug.LogError($"There is no Currency with Definition {currencyDefinition.Id} exist on {this}");
                return null;
            }
            return currencyView;
        }

        private Inventory GetInventory()
        {
            if (m_Inventory == null)
            {
                m_Inventory = PlayerSingleton.Current.Inventory;
            }
            return m_Inventory;
        }

        private ICurrency GetCurrency(string currencyId)
        {
            return GetInventory().GetCurrency(currencyId);
        }

        public void SetAmountText(ICurrency currency)
        {
            SetAmountTextInternal(currency);
        }

        private void SetAmountTextInternal(ICurrency currency)
        {
            GetCurrencyView(currency.Id).SetAmountText(currency.Amount);
        }

        public override void Show(float overideDelay = 0)
        {
            base.Show(overideDelay);
            foreach (var view in m_CurrencyViews) 
            {
                SetAmountTextInternal(GetCurrency(view.Definition.Id));
            }
        }
    }
}
