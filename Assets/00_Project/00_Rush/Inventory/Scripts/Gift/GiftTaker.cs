using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class GiftTaker : Interactable
    {
        [SerializeField]
        private GiftFavoritesDefinition m_Favorite;

        [SerializeField]
        private UnityEvent<float> m_OnGiftTaken = new();
        [SerializeField]
        private UnityEvent<Item> m_OnGiftDenied = new();

        public Item GetFavorite(Item item)
        {
            return m_Favorite.GetFavorite(item).Favorite;
        }
        public float GetValue(Item item)
        {
            return m_Favorite.GetValue(item);
        }
        public bool HasFavorite(Item item)
        {
            return m_Favorite.HasFavorite(item);
        }
        public void TakeGift(Item item)
        {
            OnGiftTakenInvoke(m_Favorite.GetValue(item));
        }
        private void OnGiftTakenInvoke(float val)
        {
            m_OnGiftTaken?.Invoke(val);
        }
        public void OnGiftDeniedInvoke(Item item)
        {
            m_OnGiftDenied?.Invoke(item);
        }
    }
}
