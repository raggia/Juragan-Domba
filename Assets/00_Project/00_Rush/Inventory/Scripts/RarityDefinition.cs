using UnityEngine;

namespace Rush
{
    [CreateAssetMenu(fileName = "New Rarity", menuName = "Rush/Inventory/Rarity")]
    public class RarityDefinition : UnitDefinition
    {
        [SerializeField]
        private Color m_Color;
        [SerializeField]
        private Sprite m_Sprite;

        public Color Color => m_Color;
        public Sprite Sprite => m_Sprite;
    }
}
