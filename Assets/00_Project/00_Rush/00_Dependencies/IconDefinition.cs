using UnityEngine;

namespace Rush
{
    public abstract class IconDefinition : UnitDefinition
    {
        [SerializeField]
        private Sprite m_Icon;
        public Sprite Icon => m_Icon;
    }
}
