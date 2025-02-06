using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rush
{

    public partial class ItemCollectView : UIView
    {
        [SerializeField, ReadOnly]
        private Item m_Item;
        [SerializeField]
        private Image m_ItemIcon;
        [SerializeField]
        private TextMeshProUGUI m_Text;

        public void SetUp(IItem item)
        {
            m_Item = new Item(item);
            m_ItemIcon.sprite = m_Item.GetIcon();

            string itemName = item.Label;
            string amountText = item.Amount.ToString();
            m_Text.text = $"{itemName} x{amountText}";
        }

        public IItem GetItem() { return m_Item; }
    }
}
