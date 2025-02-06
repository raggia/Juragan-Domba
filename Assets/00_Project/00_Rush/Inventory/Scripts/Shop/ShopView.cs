using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class ShopView : InventoryUIView
    {
        public override Inventory GetInventory()
        {
            return PlayerSingleton.Instance.GetShopInventory();
        }
        private void AddToCart(IItem item)
        {
            PlayerSingleton.Instance.AddToCart(item);
        }
        protected override void OnItemViewAdded(IItem item)
        {
            
            base.OnItemViewAdded(item);
            ShopItemView view = (ShopItemView)GetItemView(item.Id);

            view.AddActionOnShopButton(() => AddToCart(item));
        }
    }
}
