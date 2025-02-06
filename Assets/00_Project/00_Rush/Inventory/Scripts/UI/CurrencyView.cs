using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rush
{
    public partial class CurrencyView : ButtonUIView
    {
        [SerializeField]
        private Image m_Icon;
        [SerializeField]
        private CurrencyDefinition m_Definition;
        [SerializeField]
        private TextMeshProUGUI m_AmountText;

        public CurrencyDefinition Definition => m_Definition;

        protected override void Start()
        {
            base.Start();
            m_Icon.sprite = m_Definition.Icon;
        }
        public void SetAmountText(int amount)
        {
            m_AmountText.text = amount.ToString();
        }
    }
}
