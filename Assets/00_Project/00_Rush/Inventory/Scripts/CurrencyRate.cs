using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class CurrencyRate 
    {
        [SerializeField]
        private ItemDefinition m_Definition;
        [SerializeField]
        private float m_Rate;

        public ItemDefinition Definition => m_Definition;
        public float Rate => m_Rate;

        public CurrencyRate(ItemDefinition defi, float rate)
        {
            m_Definition = defi;
            m_Rate = rate;
        }
    }
}
