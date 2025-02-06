using UnityEngine;

namespace Rush
{
    public partial class BuyShopDetector : MonoBehaviour
    {
        public void SetBuyShop(GameObject target)
        {
            if (target.TryGetComponent(out ShopObject shop))
            {
                PlayerSingleton.Instance.SetShopForBuy(shop);
            }
        }
    }
}
