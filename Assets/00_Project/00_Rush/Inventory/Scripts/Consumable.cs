using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public class Consumable : ItemBehaviour
    {
        [SerializeField]
        private Stats m_StatModifier;
    }
}
