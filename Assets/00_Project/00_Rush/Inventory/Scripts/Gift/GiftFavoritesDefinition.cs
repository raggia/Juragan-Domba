using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public partial class GiftFavorite
    {
        [SerializeField]
        private Item m_Favorite;
        [SerializeField]
        private float m_Value;
        public ItemDefinition Definition => m_Favorite.Definition;
        public Item Favorite => m_Favorite;
        public float Value => m_Value;

        public int Amount => m_Favorite.Amount;

        public string FavoriteDetail => $"{m_Favorite.Label}({m_Favorite.Amount})";
    }
    [CreateAssetMenu(fileName = "New Gift Favorite", menuName = "Rush/Inventory/Gift Favorite")]
    public partial class GiftFavoritesDefinition : UnitDefinition
    {
        [SerializeField]
        private List<GiftFavorite> m_Favorites = new();
        private GiftFavorite GetFavoriteInternal(ItemDefinition defi)
        {
            GiftFavorite match = m_Favorites.Find(x => x.Definition == defi);
            if (match == null)
            {
                Debug.LogWarning($"There is no Favorite {defi} in {m_Favorites}");
                match = null;
            }
            return match;
        }
        public GiftFavorite GetFavorite(Item item)
        {
            return GetFavoriteInternal(item.Definition);
        }
        public bool HasFavorite(Item item)
        {
            bool hasItem = m_Favorites.Contains(GetFavoriteInternal(item.Definition));
            bool enoughAmount = item.Amount >= Mathf.Abs(GetFavoriteInternal(item.Definition).Amount); 
            return hasItem && enoughAmount;
        }

        public float GetValue(Item item)
        {
            return GetFavoriteInternal(item.Definition).Value;
        }

        public string GetAllFavoriteName()
        {
            string favoriteDetail = "";
            foreach (GiftFavorite favorite in m_Favorites)
            {
                favoriteDetail += favorite.FavoriteDetail;
            }
            return favoriteDetail;
        }
    }
}
